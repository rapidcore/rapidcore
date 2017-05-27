using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Xunit;

namespace RapidCore.Mongo.FunctionalTests
{
    public class MongoDbConnection_Insert_Tests : MongoConnectedTestBase
    {
        private readonly MongoDbConnection connection;
        private readonly string collectionName = "documents";

        public MongoDbConnection_Insert_Tests()
        {
            connection = new MongoDbConnection(GetDb());
        }

        [Fact]
        public async void InsertAsync_inserts()
        {
            EnsureEmptyCollection(collectionName);

            var doc = new Document { String = "insert" };
            Assert.Equal(ObjectId.Empty, doc.Id);

            await connection.InsertAsync<Document>(collectionName, doc);

            Assert.NotEqual(ObjectId.Empty, doc.Id);

            var actual = await GetDb()
                            .GetCollection<Document>(collectionName)
                            .Find(filter => filter.String == doc.String)
                            .FirstOrDefaultAsync();

            Assert.NotNull(actual);
            Assert.Equal(doc.String, actual.String);
        }

        [Fact]
        public async void InsertAsync_insertSameDocument_throws()
        {
            EnsureEmptyCollection(collectionName);

            var doc = new Document { String = "multi" };

            // first insert is ok
            await connection.InsertAsync<Document>(collectionName, doc);

            // second insert throws, as the document now has an ID
            await Assert.ThrowsAsync<MongoWriteException>(async () => await connection.InsertAsync<Document>(collectionName, doc));
        }
    }
}