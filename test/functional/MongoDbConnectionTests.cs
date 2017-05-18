using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Xunit;

namespace RapidCore.Mongo.FunctionalTests
{
    public class MongoDbConnectionTests : MongoConnectedTestBase
    {
        private readonly MongoDbConnection connection;
        private readonly string collectionName = "documents";
        public MongoDbConnectionTests()
        {
            connection = new MongoDbConnection(GetDb());
        }

        [Fact]
        public async Task FirstOrDefaultAsync_ReturnsDefault_ifNoResults()
        {
            EnsureEmptyCollection(collectionName);
            
            var actual = await connection.FirstOrDefaultAsync<Document>(collectionName, filter => filter.String == "does not exist");

            Assert.Null(actual);
        }

        [Fact]
        public async Task FirstOrDefaultAsync_ReturnsInstance_if_1_result()
        {
            EnsureEmptyCollection(collectionName);

            var doc = new Document { String = "one" };
            await GetDb().GetCollection<Document>(collectionName).InsertOneAsync(doc);
            
            var actual = await connection.FirstOrDefaultAsync<Document>(collectionName, filter => filter.String == "one");

            Assert.NotNull(actual);
            Assert.Equal("one", actual.String);
        }

        [Fact]
        public async Task FirstOrDefaultAsync_ReturnsFirstInstance_if_moreThanOneResult()
        {
            EnsureEmptyCollection(collectionName);

            var first = new Document { String = "one", Aux = "first" };
            Insert<Document>(collectionName, first);

            var second = new Document { String = "one", Aux = "second" };
            Insert<Document>(collectionName, second);
            
            var actual = await connection.FirstOrDefaultAsync<Document>(collectionName, filter => filter.String == "one");

            Assert.NotNull(actual);
            Assert.Equal("one", actual.String);
            Assert.Equal("first", actual.Aux);
        }

        [Fact]
        public async Task Upsert_Can_Insert_and_Update()
        {
            EnsureEmptyCollection(collectionName);

            var doc = new Document { String = "hi" };
            await connection.UpsertAsync<Document>(collectionName, doc, filter => filter.String == "hi");

            Assert.NotNull(doc.Id);

            var foundDocument = GetDb().GetCollection<Document>(collectionName).Find(filter => filter.String == "hi").FirstOrDefault();

            Assert.NotNull(foundDocument);

            foundDocument.String = "bye";

            await connection.UpsertAsync<Document>(collectionName, foundDocument, filter => filter.Id == foundDocument.Id);

            var updatedDocument = GetDb().GetCollection<Document>(collectionName).Find(filter => filter.Id == foundDocument.Id).FirstOrDefault();

            Assert.NotNull(updatedDocument);
            Assert.Equal("bye", updatedDocument.String);
        }

        #region Test document
        private class Document
        {
            [BsonIgnoreIfDefault]
            public ObjectId Id { get; set; }

            public string String { get; set; }

            public string Aux { get; set; }
        }
        #endregion
    }
}
