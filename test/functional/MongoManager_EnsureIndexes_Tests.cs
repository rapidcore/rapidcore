using System;
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
            var actual = CreateAndGetIndexes<SimpleIndexes>();

            Assert.Equal(3, actual.Count); // the auto-generated "_id_" and our own
            Assert.Equal(new BsonDocument().Add("String", 1), actual["string_index"].GetElement("key").Value);
            Assert.Equal(new BsonDocument().Add("Int", 1), actual["Int_1"].GetElement("key").Value);
        }

        [Fact]
        public void CanCreateCompoundIndexes()
        {
            var actual = CreateAndGetIndexes<CompoundIndexes>();

            Assert.Equal(2, actual.Count); // the auto-generated "_id_" and our own
            Assert.Equal(new BsonDocument().Add("CompoundnessOfTheLong", 1).Add("StringOfCompoundness", 1), actual["compound_index"].GetElement("key").Value);
        }

        [Fact]
        public void CreatesIndexesFromAllLevels()
        {
            var actual = CreateAndGetIndexes<MultiLevelTwo>();

            Assert.Equal(4, actual.Count); // the auto-generated "_id_" and our own + compound
            Assert.Equal(new BsonDocument().Add("OnOne", 1), actual["OnOne_1"].GetElement("key").Value);
            Assert.Equal(new BsonDocument().Add("OnTwo", 1), actual["OnTwo_1"].GetElement("key").Value);
        }

        [Fact]
        public void CreatesMultiLevelCompoundIndexes()
        {
            var actual = CreateAndGetIndexes<MultiLevelTwo>();

            Assert.Equal(4, actual.Count); // the auto-generated "_id_" and our own + compound
            Assert.Equal(new BsonDocument().Add("OnTwo", 1).Add("OnOne", 1), actual["multi_level"].GetElement("key").Value);
        }

        [Fact]
        public void CreatesIndexesOnNestedTypes()
        {
            var actual = CreateAndGetIndexes<Envelope>();

            Assert.Equal(2, actual.Count); // the auto-generated "_id_" and our own
            Assert.Equal(new BsonDocument().Add("Nested.OnNested", 1), actual["Nested.OnNested_1"].GetElement("key").Value);
        }

        #region Create and get indexes
        private IDictionary<string, BsonDocument> CreateAndGetIndexes<TDocument>()
        {
            var collectionName = typeof(TDocument).GetTypeInfo().GetCollectionName();
            GetDb().DropCollection(collectionName);
            GetDb().CreateCollection(collectionName);

            manager.EnsureIndexes(
                GetDb(),
                typeof(TDocument)
            );

            return GetIndexes<TDocument>(collectionName);
        }
        #endregion

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
        [Entity]
        private class SimpleIndexes
        {
            [Index("string_index")]
            public string String { get; set; }

            [Index]
            public int Int { get; set; }
        }
        #endregion

        #region Compound indexes
        [Entity]
        private class CompoundIndexes
        {
            [Index("compound_index", Order = 2)]
            public string StringOfCompoundness { get; set; }

            [Index("compound_index", Order = 1)]
            public long CompoundnessOfTheLong { get; set; }
        }
        #endregion

        #region Multi level
        private class MultiLevelOne
        {
            [Index]
            [Index("multi_level")]
            public string OnOne { get; set; }
        }

        [Entity]
        private class MultiLevelTwo : MultiLevelOne
        {
            [Index]
            [Index("multi_level")]
            public string OnTwo { get; set; }
        }
        #endregion

        #region Nested types
        [Entity]
        private class Envelope
        {
            public Nested Nested { get; set; }
        }

        private class Nested
        {
            [Index]
            public string OnNested { get; set; }
        }
        #endregion
    }
}