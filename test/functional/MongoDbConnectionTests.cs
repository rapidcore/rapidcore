using System;
using System.Linq;
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
        public async Task UpsertAsync_Can_Insert_and_Update()
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

        [Fact]
        public async Task DeleteAsync_deletesAllMatches()
        {
            EnsureEmptyCollection(collectionName);

            Insert<Document>(collectionName, new Document { String = "one", Aux = "deleteMe" });
            Insert<Document>(collectionName, new Document { String = "two", Aux = "keep" });
            Insert<Document>(collectionName, new Document { String = "thr", Aux = "deleteMe" });
            
            await connection.DeleteAsync<Document>(collectionName, filter => filter.Aux == "deleteMe");

            var actual = GetAll<Document>(collectionName);

            Assert.Equal(1, actual.Count);
            Assert.Equal("two", actual[0].String);
        }

        [Fact]
        public async Task GetAsync_ReturnsAllMatches_WhenNotGivenLimit()
        {
            EnsureEmptyCollection(collectionName);

            Insert<Document>(collectionName, new Document { String = "one", Aux = "mememe" });
            Insert<Document>(collectionName, new Document { String = "two", Aux = "hipster" });
            Insert<Document>(collectionName, new Document { String = "thr", Aux = "mememe" });

            var actual = await connection.GetAsync<Document>(collectionName, filter => filter.Aux == "mememe");

            Assert.Equal(2, actual.Count());
            Assert.Equal("one", actual.ElementAt(0).String);
            Assert.Equal("thr", actual.ElementAt(1).String);
        }

        [Fact]
        public async Task GetAsync_UsesLimit()
        {
            EnsureEmptyCollection(collectionName);

            Insert<Document>(collectionName, new Document { String = "one", Aux = "mememe" });
            Insert<Document>(collectionName, new Document { String = "two", Aux = "hipster" });
            Insert<Document>(collectionName, new Document { String = "thr", Aux = "mememe" });

            var actual = await connection.GetAsync<Document>(collectionName, filter => filter.Aux == "mememe", 1);

            Assert.Equal(1, actual.Count());
            Assert.Equal("one", actual.ElementAt(0).String);
        }

        [Fact]
        public async Task GetAsync_ReturnsEmptyList_ifNoResults()
        {
            EnsureEmptyCollection(collectionName);

            var actual = await connection.GetAsync<Document>(collectionName, filter => true, 1);

            Assert.Empty(actual);
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
