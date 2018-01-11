using System;
using System.Reflection;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Reflection
{
    public class TypeGetMethodRecursivelyExtensionsTests
    {
        [Fact]
        public void GetMethodRecursively_FindsMethodsWithParams_InFirstLayer()
        {
            var actual = typeof(GuineaPig).GetMethodRecursively("OneParam", typeof(string));
            
            Assert.NotNull(actual);
            Assert.Equal("OneParam", actual.Name);
        }

        [Fact]
        public void GetMethodRecursively_FindsMethodsWithoutParams_InFirstLayer()
        {
            var actual = typeof(GuineaPig).GetMethodRecursively("ZeroParams");
            
            Assert.NotNull(actual);
            Assert.Equal("ZeroParams", actual.Name);
        }

        [Fact]
        public void GetMethodRecursively_FindsOverloadedMethodsWithoutParams()
        {
            var actual = typeof(GuineaPig).GetMethodRecursively("HasOverload");
            
            Assert.NotNull(actual);
            Assert.Equal("HasOverload", actual.Name);
            Assert.Empty(actual.GetParameters());
        }

        [Fact]
        public void GetMethodRecursively_FindsOverloadedMethodsWithParams()
        {
            var actual = typeof(GuineaPig).GetMethodRecursively("HasOverload", typeof(Attribute));
            
            Assert.NotNull(actual);
            Assert.Equal("HasOverload", actual.Name);
            Assert.Equal(1, actual.GetParameters().Length);
        }

        [Fact]
        public void GetMethodRecursively_FindsMethodsWithoutParams_InNextLayer()
        {
            var actual = typeof(GuineaPig).GetMethodRecursively("GetType");
            
            Assert.NotNull(actual);
            Assert.Equal("GetType", actual.Name);
            Assert.Equal(typeof(Object), actual.DeclaringType);
        }

        [Fact]
        public void GetMethodRecursively_Throws_ifMethodDoesNotExist_withoutParams()
        {

            var ex = Assert.Throws<MissingMethodException>(() => typeof(GuineaPig).GetMethodRecursively("DoesNotExist"));

            Assert.Equal("Could not find method DoesNotExist()", ex.Message);
        }

        [Fact]
        public void GetMethodRecursively_Throws_ifMethodDoesNotExist_singleParam()
        {
            var ex = Assert.Throws<MissingMethodException>(() => typeof(GuineaPig).GetMethodRecursively("DoesNotExist", typeof(long)));

            Assert.Equal("Could not find method DoesNotExist(Int64)", ex.Message);
        }

        [Fact]
        public void GetMethodRecursively_Throws_ifMethodDoesNotExist_multipleParams()
        {

            var ex = Assert.Throws<MissingMethodException>(() => typeof(GuineaPig).GetMethodRecursively("DoesNotExist", typeof(long), typeof(int), typeof(Attribute)));

            Assert.Equal("Could not find method DoesNotExist(Int64, Int32, Attribute)", ex.Message);
        }

        [Fact]
        public void GetMethodRecursively_Throws_ifMethodDoesNotExist_withThoseParameters()
        {
            var ex = Assert.Throws<MissingMethodException>(() => typeof(GuineaPig).GetMethodRecursively("HasOverload", typeof(long), typeof(int), typeof(Attribute)));

            Assert.Equal("Could not find method HasOverload(Int64, Int32, Attribute)", ex.Message);
        }

        #region GuineaPig
        private class GuineaPig
        {
            public void OneParam(string a) { }

            public void ZeroParams() { }

            public void HasOverload() { }

            public void HasOverload(Attribute attrib) { }
        }
        #endregion
    }
}