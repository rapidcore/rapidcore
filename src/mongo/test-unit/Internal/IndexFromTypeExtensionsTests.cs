using System;
using System.Reflection;
using MongoDB.Bson;
using RapidCore.Mongo.Internal;
using Xunit;

namespace RapidCore.Mongo.UnitTests.Internal
{
    public class IndexFromTypeExtensionsTests
    {
        [Fact]
        public void DetectsRecursionAndStops()
        {
            var actual = Assert.Throws<InvalidOperationException>(() => typeof(RecursiveParent).GetTypeInfo().GetIndexDefinitions());

            Assert.Equal("Tree is too deep - could be a recursion. Current 'path' is Child.Parent.Child.Parent.Child.Parent.Child.Parent.Child.Parent.Child.Parent.Child.Parent.Child.Parent.Child.Parent.Child.Parent.", actual.Message);
        }

        [Fact]
        public void DealsWithTheInsanityOfObjectId()
        {
            var actual = typeof(WithObjectId).GetTypeInfo().GetIndexDefinitions();

            Assert.Equal(1, actual.Count);
            Assert.Equal(1, actual[0].Keys.Count);
            Assert.Equal("ThisIsOk", actual[0].Keys[0].Name);
        }
        
        #region Recursive
        [Entity]
        private class RecursiveParent
        {
            public RecursiveChild Child { get; set; }
        }

        private class RecursiveChild
        {
            public RecursiveParent Parent { get; set; }
        }
        #endregion

        #region Mongo ObjectId
        [Entity]
        private class WithObjectId
        {
            public ObjectId Id { get; set; }

            [Index]
            public string ThisIsOk { get; set; }
        }
        #endregion
    }
}