using System;
using System.Reflection;
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
    }
}