using RapidCore.Reflection;
using Xunit;

namespace UnitTests.Core.Reflection
{
    public class InstanceTraversalContextTests
    {
        private readonly InstanceTraversalContext context;

        public InstanceTraversalContextTests()
        {
            context = new InstanceTraversalContext();
        }

        #region CurrentDepth
        [Fact]
        public void CurrentDepth_worksWithDefaultInstance()
        {
            Assert.Equal(0, new InstanceTraversalContext().CurrentDepth);
        }
        
        [Fact]
        public void CurrentDepth_worksWithNullBreadcrumb()
        {
            context.BreadcrumbStack = null;
            
            Assert.Equal(0, context.CurrentDepth);
        }
        
        [Fact]
        public void CurrentDepth_returnsNumberOfElementsInBreadcrumb()
        {
            context.BreadcrumbStack.Push("one");
            context.BreadcrumbStack.Push("two");
            context.BreadcrumbStack.Push("three");
            context.BreadcrumbStack.Push("four");
            
            Assert.Equal(4, context.CurrentDepth);
        }
        #endregion

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        [InlineData(4, true)]
        public void CanGoDeeper(int maxDepth, bool expected)
        {
            context.MaxDepth = maxDepth;
            
            context.BreadcrumbStack.Push("one");
            context.BreadcrumbStack.Push("two");
            context.BreadcrumbStack.Push("three");
            
            Assert.Equal(expected, context.CanGoDeeper());
        }

        #region BreadcrumbAsString
        [Fact]
        public void BreadcrumbAsString_worksWithDefaultInstance()
        {
            Assert.Equal(string.Empty, new InstanceTraversalContext().BreadcrumbAsString);
        }
        
        [Fact]
        public void BreadcrumbAsString_worksWhenBreadcrumbIsNull()
        {
            context.BreadcrumbStack = null;
            
            Assert.Equal(string.Empty, context.BreadcrumbAsString);
        }

        [Theory]
        [InlineData("one", new[] {"one"})]
        [InlineData("one.two.three", new[] {"one", "two", "three"})]
        public void BreadcrumbAsString(string expected, string[] pushElements)
        {
            foreach (var pushElement in pushElements)
            {
                context.BreadcrumbStack.Push(pushElement);
            }
            
            Assert.Equal(expected, context.BreadcrumbAsString);
        }
        #endregion
    }
}