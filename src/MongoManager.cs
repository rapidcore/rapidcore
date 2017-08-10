using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using MongoDB.Driver;
using RapidCore.Mongo.Internal;
using RapidCore.Reflection;

namespace RapidCore.Mongo
{
    public class MongoManager
    {
        public void EnsureIndexes(IMongoDatabase lowLevelDb, Assembly assembly, string entityNamespace)
        {
            // get types flagged with [Entity]
            var types = assembly
                .DefinedTypes
                .Where(t => t.HasAttribute(typeof(EntityAttribute)) && t.Namespace == entityNamespace)
                .Select(t => t);

            foreach (var type in types)
            {
                EnsureIndexes(lowLevelDb, type);
            }
        }

        public void EnsureIndexes(IMongoDatabase lowLevelDb, Type type)
        {
            EnsureIndexes(lowLevelDb, type.GetTypeInfo());
        }

        public void EnsureIndexes(IMongoDatabase lowLevelDb, TypeInfo type)
        {
            type.GetIndexDefinitions().ToList().ForEach(index =>
            {
                try
                {
                    CreateIndex(lowLevelDb, index);
                }
                catch (TargetInvocationException ex) when (ex.InnerException is MongoCommandException && ((MongoCommandException)ex.InnerException).EnsureIndexesShouldDropAndRetry(index))
                {
                    DropIndex(lowLevelDb, index);
                    CreateIndex(lowLevelDb, index);
                }
            });
        }

        private void CreateIndex(IMongoDatabase lowLevelDb, IndexDefinition index)
        {
            // make the following call:
            // lowLevelDb.GetCollection<TDocument>("collectionName").Indexes.CreateOne(index.GetKeySpec(), index.GetOptions())

            var collectionIndexes = GetCollectionIndexesManager(lowLevelDb, index);

            collectionIndexes
                    .GetType()
                    .GetMethodRecursively(
                        "CreateOne",
                        typeof(IndexKeysDefinition<>).MakeGenericType(new Type[] { index.DocumentType }),
                        typeof(CreateIndexOptions),
                        typeof(CancellationToken)
                    )
                    .Invoke(collectionIndexes, new object[] { index.GetKeySpec(), index.GetOptions(), null });
        }

        private void DropIndex(IMongoDatabase lowLevelDb, IndexDefinition index)
        {
            // make the following call
            // lowLevelDb.GetCollection<T>("name").Indexes.DropOne(index.Collection);

            var collectionIndexes = GetCollectionIndexesManager(lowLevelDb, index);

            collectionIndexes
                    .GetType()
                    .GetMethodRecursively(
                        "DropOne",
                        typeof(string),
                        typeof(CancellationToken)
                    )
                    .Invoke(collectionIndexes, new object[] { index.Name, null });
        }

        private object GetCollectionIndexesManager(IMongoDatabase lowLevelDb, IndexDefinition index)
        {
            var mongoCollection = lowLevelDb
                .GetType()
                .GetMethodRecursively("GetCollection", typeof(string), typeof(MongoCollectionSettings))
                .MakeGenericMethod(new Type[] { index.DocumentType })
                .Invoke(lowLevelDb, new object[] { index.Collection, null });

            return mongoCollection.InvokeGetterRecursively("Indexes");
        }
    }
}