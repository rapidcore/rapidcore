using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using RapidCore.Mongo.Testing;
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
            var actual = connection.GetCollection<DocumentWithCollectionSpecified>("kewl_stuff");

            Assert.IsAssignableFrom<IMongoCollection<DocumentWithCollectionSpecified>>(actual);
            Assert.Equal("kewl_stuff", actual.CollectionNamespace.CollectionName);
        }

        [Fact]
        public void GetCollection_usesCollectionNameDefinedInEntityAttribute_ifParamIsNull()
        {
            var actual = connection.GetCollection<DocumentWithCollectionSpecified>(null);

            Assert.IsAssignableFrom<IMongoCollection<DocumentWithCollectionSpecified>>(actual);
            Assert.Equal("SomeCollection", actual.CollectionNamespace.CollectionName);
        }

        [Fact]
        public void GetCollection_usesNameOfDocumentType_ifNoCollectionNameSpecifiedInEntityAttribute_ifParamIsNull()
        {
            var actual = connection.GetCollection<DocumentWithNoCollectionSpecified>(null);

            Assert.IsAssignableFrom<IMongoCollection<DocumentWithNoCollectionSpecified>>(actual);
            Assert.Equal("DocumentWithNoCollectionSpecified", actual.CollectionNamespace.CollectionName);
        }
        
        [Fact]
        public void GetCollection_usesCollectionNameDefinedInEntityAttribute()
        {
            var actual = connection.GetCollection<DocumentWithCollectionSpecified>();

            Assert.IsAssignableFrom<IMongoCollection<DocumentWithCollectionSpecified>>(actual);
            Assert.Equal("SomeCollection", actual.CollectionNamespace.CollectionName);
        }

        [Fact]
        public void GetCollection_usesNameOfDocumentType_ifNoCollectionNameSpecifiedInEntityAttribute()
        {
            var actual = connection.GetCollection<DocumentWithNoCollectionSpecified>();

            Assert.IsAssignableFrom<IMongoCollection<DocumentWithNoCollectionSpecified>>(actual);
            Assert.Equal("DocumentWithNoCollectionSpecified", actual.CollectionNamespace.CollectionName);
        }
        
        [Entity(CollectionName = "SomeCollection")]
        private class DocumentWithCollectionSpecified
        {
            [BsonIgnoreIfDefault]
            public ObjectId Id { get; set; }
        }
        
        private class DocumentWithNoCollectionSpecified
        {
            [BsonIgnoreIfDefault]
            public ObjectId Id { get; set; }
        }
    }
}