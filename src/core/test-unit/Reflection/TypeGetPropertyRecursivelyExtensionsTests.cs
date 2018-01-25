using System;
using System.Reflection;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Reflection
{
    public class TypeGetPropertyRecursivelyExtensionsTests
    {
        [Fact]
        public void GetPropertyRecursively_canFindIt_inFirstLayer()
        {
            var actual = typeof(GuineaPig).GetPropertyRecursively("FirstLayer");

            Assert.Equal("FirstLayer", actual.Name);
        }

        [Fact]
        public void GetPropertyRecursively_canFindIt_inNextLayer()
        {
            var actual = typeof(GuineaPig).GetPropertyRecursively("SecondLayer");

            Assert.Equal("SecondLayer", actual.Name);
        }

        [Fact]
        public void GetPropertyRecursively_throwsIfNoSuchProperty()
        {
            var ex = Assert.Throws<MissingMemberException>(() => typeof(GuineaPig).GetPropertyRecursively("DoesNotExist"));

            Assert.Equal("Could not find a property called DoesNotExist", ex.Message);
        }

        #region GuineaPig
        private class GuineaPig : GuineaPigBase
        {
            public string FirstLayer { get; set; }
        }

        private abstract class GuineaPigBase
        {
            public int SecondLayer { get; set; }
        }
        #endregion
    }
}