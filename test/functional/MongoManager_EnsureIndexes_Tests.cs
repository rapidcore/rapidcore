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

            var indexes = GetIndexes<SimpleIndexes>(collectionName);

            Assert.Equal(3, indexes.Count); // the auto-generated "_id_" and our own
            Assert.Equal(new BsonDocument().Add("String", 1), indexes["string_index"].GetElement("key").Value);
            Assert.Equal(new BsonDocument().Add("Int", 1), indexes["Int_1"].GetElement("key").Value);
        }

        [Fact]
        public void CanCreateCompoundIndexes()
        {
            var collectionName = typeof(CompoundIndexes).GetTypeInfo().GetCollectionName();
            GetDb().DropCollection(collectionName);
            GetDb().CreateCollection(collectionName);

            manager.EnsureIndexes(
                GetDb(),
                typeof(CompoundIndexes).GetTypeInfo().Assembly,
                typeof(CompoundIndexes).GetTypeInfo().Namespace
            );

            var indexes = GetIndexes<CompoundIndexes>(collectionName);

            Assert.Equal(2, indexes.Count); // the auto-generated "_id_" and our own
            Assert.Equal(new BsonDocument().Add("CompoundnessOfTheLong", 1).Add("StringOfCompoundness", 1), indexes["compound_index"].GetElement("key").Value);
        }

        #region GetIndexes
        private IDictionary<string, BsonDocument> GetIndexes<TDocument>(string collectionName)
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

        #region Simple indexes
        private class SimpleIndexes
        {
            [Index("string_index")]
            public string String { get; set; }

            [Index]
            public int Int { get; set; }
        }
        #endregion

        #region Compound indexes
        private class CompoundIndexes
        {
            [Index("compound_index", Order = 2)]
            public string StringOfCompoundness { get; set; }

            [Index("compound_index", Order = 1)]
            public long CompoundnessOfTheLong { get; set; }
        }
        #endregion
    }
}