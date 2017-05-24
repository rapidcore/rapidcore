using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Logging;
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
                .Where(t => t.HasAttribute(typeof(EntityAttribute)))
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
                // make the following call:
                // lowLevelDb.GetCollection<TDocument>("collectionName").Indexes.CreateOne(index.GetKeySpec(), index.GetOptions())

                var genericDocType = new Type[] { index.DocumentType };

                var mongoCollection = lowLevelDb
                    .GetType()
                    .GetMethodRecursively("GetCollection", typeof(string), typeof(MongoCollectionSettings))
                    .MakeGenericMethod(genericDocType)
                    .Invoke(lowLevelDb, new object[] { index.Collection, null });

                var collectionIndexes = mongoCollection.InvokeGetterRecursively("Indexes");

                collectionIndexes
                    .GetType()
                    .GetMethodRecursively(
                        "CreateOne",
                        typeof(IndexKeysDefinition<>).MakeGenericType(genericDocType),
                        typeof(CreateIndexOptions),
                        typeof(CancellationToken)
                    )
                    .Invoke(collectionIndexes, new object[] { index.GetKeySpec(), index.GetOptions(), null });
            });
        }
    }
}