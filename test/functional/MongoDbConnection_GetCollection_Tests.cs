using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Xunit;

namespace RapidCore.Mongo.FunctionalTests
{
    public class MongoDbConnection_GetCollection_Tests : MongoConnectedTestBase
    {
        private readonly MongoDbConnection connection;

        public MongoDbConnection_GetCollection_Tests()
        {
            connection = new MongoDbConnection(GetDb());
        }

        [Fact]
        public void GetCollection_usesGivenCollectionName()
        {
            var actual = connection.GetCollection<Document>("kewl_stuff");

            Assert.IsAssignableFrom<IMongoCollection<Document>>(actual);
            Assert.Equal("kewl_stuff", actual.CollectionNamespace.CollectionName);
        }

        [Fact]
        public void GetCollection_usesNameOfDocumentType_ifNotGivenACollectionName()
        {
            var actual = connection.GetCollection<Document>();

            Assert.IsAssignableFrom<IMongoCollection<Document>>(actual);
            Assert.Equal("Document", actual.CollectionNamespace.CollectionName);
        }
    }
}