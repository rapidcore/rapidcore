using System.Collections.Generic;
using MongoDB.Driver;

namespace RapidCore.Mongo.FunctionalTests
{
    public abstract class MongoConnectedTestBase
    {
        private MongoClient lowLevelClient;
        private IMongoDatabase db;
        private bool isConnected = false;

        protected string GetDbName()
        {
            return GetType().Name;
        }

        protected void Connect(string connectionString = "mongodb://localhost:27017")
        {
            lowLevelClient = new MongoClient(connectionString);
            lowLevelClient.DropDatabase(GetDbName());
            db = lowLevelClient.GetDatabase(GetDbName());
        }

        protected MongoClient GetClient()
        {
            if (!isConnected)
            {
                Connect();
                isConnected = true;
            }

            return lowLevelClient;
        }

        protected IMongoDatabase GetDb()
        {
            return GetClient().GetDatabase(GetDbName());
        }

        protected void EnsureEmptyCollection(string collectionName)
        {
            GetDb().DropCollection(collectionName);
        }

        protected void Insert<TDocument>(string collectionName, TDocument doc)
        {
            GetDb().GetCollection<TDocument>(collectionName).InsertOne(doc);
        }

        protected IList<TDocument> GetAll<TDocument>(string collectionName)
        {
            return GetDb().GetCollection<TDocument>(collectionName).Find(filter => true).ToList();
        }
    }
}