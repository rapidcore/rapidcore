using System.Collections.Generic;
using MongoDB.Driver;

namespace RapidCore.Mongo.Testing
{
    /// <summary>
    /// Base class for functional tests that need access to
    /// a Mongo database.
    /// 
    /// It provides simple helpers that we use ourselves.
    /// </summary>
    public abstract class MongoConnectedTestBase
    {
        private MongoClient lowLevelClient;
        private IMongoDatabase db;
        private bool isConnected = false;

        protected string ConnectionString { get; set; } = "mongodb://localhost:27017";

        protected string GetDbName()
        {
            return GetType().Name;
        }

        protected void Connect()
        {
            if (!isConnected)
            {
                lowLevelClient = new MongoClient(ConnectionString);
                lowLevelClient.DropDatabase(GetDbName());
                db = lowLevelClient.GetDatabase(GetDbName());
                isConnected = true;
            }
        }

        protected MongoClient GetClient()
        {
            Connect();
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