using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver;
using RapidCore.Mongo.Internal;
using Xunit;

namespace RapidCore.Mongo.FunctionalTests.MongoManagerTests
{
    public class EnsureIndexesWithCustomCollectionNameTests : MongoManagerTestsBase
    {
        [Fact]
        public void CanCreateSimpleIndexes()
        {
            var actual = CreateAndGetIndexes<SimpleIndexes>();

            Assert.Equal(3, actual.Count); // the auto-generated "_id_" and our own
            Assert.Equal(new BsonDocument().Add("String", 1), actual["string_index"].GetElement("key").Value);
            Assert.Equal(new BsonDocument().Add("Int", 1), actual["Int_1"].GetElement("key").Value);
        }

        [Fact]
        public void DoNothingIfTheIndexAlreadyExists_andIsTheSame()
        {
            CreateAndGetIndexes<SimpleIndexes>(); // should drop and create
            var afterSecondRun = CreateAndGetIndexes<SimpleIndexes>(false); // should _NOT_ drop

            Assert.Equal(3, afterSecondRun.Count); // the auto-generated "_id_" and our own
            Assert.Equal(new BsonDocument().Add("String", 1), afterSecondRun["string_index"].GetElement("key").Value);
            Assert.Equal(new BsonDocument().Add("Int", 1), afterSecondRun["Int_1"].GetElement("key").Value);
        }

        #region Simple indexes
        [Entity(CollectionName = "Simples")]
        private class SimpleIndexes
        {
            [Index("string_index")]
            public string String { get; set; }

            [Index]
            public int Int { get; set; }
        }
        #endregion
    }
}