using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RapidCore.Mongo.Internal;
using RapidCore.Reflection;
using RapidCore.Reflection.Extensions;

namespace RapidCore.Mongo
{
    public class MongoManager
    {
        public void EnsureIndexes(IMongoDatabase lowLevelDb, Assembly assembly, string entityNamespace)
        {
            var types = from t in assembly.DefinedTypes
                        from p in t.GetProperties()
                        where t.Namespace == entityNamespace
                        where p.HasAttribute(typeof(IndexAttribute))
                        select t;

            foreach (var type in types)
            {
                type.GetIndexDefinitions().ForEach(index =>
                {
                    // make the following call:
                    // lowLevelDb.GetCollection<TDocument>("collectionName").Indexes.CreateOne(index.GetKeySpec(), index.GetOptions())
                    var mongoCollection = lowLevelDb
                        .GetType()
                        .GetTypeInfo()
                        .GetDeclaredMethod("GetCollection")
                        .MakeGenericMethod(new Type[] { index.DocumentType })
                        .Invoke(lowLevelDb, new object[] { index.Collection, null });

                    var collectionIndexes = mongoCollection
                        .GetType()
                        .GetTypeInfo()
                        .GetDeclaredProperty("Indexes")
                        .GetMethod
                        .Invoke(mongoCollection, new object[0]);

                    var ciType = collectionIndexes.GetType();
                    var ciTypeInfo = ciType.GetTypeInfo().BaseType.GetTypeInfo();

                    collectionIndexes
                        .GetType()
                        .GetTypeInfo()
                        .BaseType // because CreateOne is not declared on MongoIndexManager, but on the base class
                        .GetTypeInfo()
                        .GetDeclaredMethod("CreateOne")
                        .Invoke(collectionIndexes, new object[] { index.GetKeySpec(), index.GetOptions(), null });
                });
            }
        }
    }
}