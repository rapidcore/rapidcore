using System.Collections.Generic;
using System.Reflection;
using MongoDB.Bson;
using RapidCore.Mongo.Internal;
using Xunit;

namespace RapidCore.Mongo.FunctionalTests
{
    public class MongoManager_EnsureIndexes_Tests : MongoConnectedTestBase
    {
        private readonly MongoManager manager;

        public MongoManager_EnsureIndexes_Tests()
        {
            manager = new MongoManager();
        }

        [Fact]
        public void CanCreateSimpleIndexes()
        {
            var collectionName = typeof(SimpleIndexes).GetTypeInfo().GetCollectionName();
            GetDb().DropCollection(collectionName);
            GetDb().CreateCollection(collectionName);

            manager.EnsureIndexes(
                GetDb(),
                typeof(SimpleIndexes).GetTypeInfo().Assembly,
                typeof(SimpleIndexes).GetTypeInfo().Namespace
            );

            var actual = GetDb().GetCollection<SimpleIndexes>(collectionName).Indexes.List();
            actual.MoveNext();
            
            var indexes = new Dictionary<string, BsonDocument>();

            foreach (var index in actual.Current)
            {
                indexes[index.GetElement("name").Value.AsString] = index;
            }

            Assert.Equal(4, indexes.Count); // the auto-generated "_id_" and our own
            Assert.Equal(new BsonDocument().Add("String", 1), indexes["string_index"].GetElement("key").Value);
            Assert.Equal(new BsonDocument().Add("Int", 1), indexes["Int_1"].GetElement("key").Value);
            Assert.Equal(new BsonDocument().Add("Int", 1).Add("String", 1), indexes["compound_index"].GetElement("key").Value);
        }

        #region Simple indexes
        private class SimpleIndexes
        {
            [Index("string_index")]
            [Index("compound_index", Order = 2)]
            public string String { get; set; }

            [Index]
            [Index("compound_index", Order = 1)]
            public int Int { get; set; }
        }
        #endregion
    }
}