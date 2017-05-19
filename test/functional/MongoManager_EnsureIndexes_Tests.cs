using System.Collections.Generic;
using System.Reflection;
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
            
            var expectedIndexes = new Dictionary<string, bool> {
                { "String", false },
                { "Int", false }
            };
            var numberOfIndexes = 0;
            foreach (var index in actual.Current)
            {
                Assert.Equal(1, index.ElementCount);
                numberOfIndexes += 1;

                expectedIndexes[index.GetElement(0).Name] = true;
            }

            Assert.Equal(2, numberOfIndexes);
            Assert.True(expectedIndexes["String"]);
            Assert.True(expectedIndexes["Int"]);
        }

        #region Simple indexes
        private class SimpleIndexes
        {
            [Index("string_index")]
            public string String { get; set; }

            [Index("int_index")]
            public int Int { get; set; }
        }
        #endregion
    }
}