using System.Collections.Generic;
using System.Reflection;
using MongoDB.Bson;
using RapidCore.Mongo.Internal;
using RapidCore.Mongo.Testing;

namespace RapidCore.Mongo.FunctionalTests.MongoManagerTests
{
    public class MongoManagerTestsBase : MongoConnectedTestBase
    {
        protected readonly MongoManager manager;

        public MongoManagerTestsBase()
        {
            manager = new MongoManager();
        }
        
        #region Create and get indexes
        protected IDictionary<string, BsonDocument> CreateAndGetIndexes<TDocument>(bool doDrop = true)
        {
            var collectionName = typeof(TDocument).GetTypeInfo().GetCollectionName();

            if (doDrop)
            {
                GetDb().DropCollection(collectionName);
                GetDb().CreateCollection(collectionName);
            }

            manager.EnsureIndexes(
                GetDb(),
                typeof(TDocument)
            );

            return GetIndexes<TDocument>(collectionName);
        }
        #endregion

        #region GetIndexes
        protected IDictionary<string, BsonDocument> GetIndexes<TDocument>(string collectionName)
        {
            var actual = GetDb().GetCollection<TDocument>(collectionName).Indexes.List();
            actual.MoveNext();
            
            var indexes = new Dictionary<string, BsonDocument>();

            foreach (var index in actual.Current)
            {
                indexes[index.GetElement("name").Value.AsString] = index;
            }

            return indexes;
        }
        #endregion
    }
}