using System;
using System.Reflection;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Reflection
{
    public class ObjectMethodInvokeExtensionsTests
    {
        private readonly GuineaPig guineaPig;

        public ObjectMethodInvokeExtensionsTests()
        {
            guineaPig = new GuineaPig();
        }

        [Fact]
        public void InvokeMethodRecursively_CanInvoke_void_withOneParam()
        {
            var actual = guineaPig.InvokeMethodRecursively("OneParam", "hi");

            Assert.Null(actual);
        }

        [Fact]
        public void InvokeMethodRecursively_CanInvoke_void_withNoParams()
        {
            var actual = guineaPig.InvokeMethodRecursively("ZeroParams");

            Assert.Null(actual);
        }

        [Fact]
        public void InvokeMethodRecursively_CanInvoke_withOneParam_andReturnValue()
        {
            var actual = guineaPig.InvokeMethodRecursively("OneParamWithReturn", "hi");

            Assert.Equal("hi", actual);
        }

        [Fact]
        public void InvokeMethodRecursively_CanInvoke_withNoParams_andReturnValue()
        {
            var actual = guineaPig.InvokeMethodRecursively("ZeroParamsWithReturn");

            Assert.Equal(666, actual);
        }

        [Fact]
        public void InvokeMethodRecursively_CanInvoke_inNextLayer()
        {
            var actual = guineaPig.InvokeMethodRecursively("GetType");

            Assert.IsAssignableFrom(typeof(Type), actual.GetType());
        }

        [Fact]
        public void InvokeGenericMethodRecursively_firstLayer()
        {
            var actual = guineaPig.InvokeGenericMethodRecursively("Generic", new Type[] { typeof(Attribute) }, "hello from generic");

            Assert.Equal("hello from generic Attribute", actual);
        }

        [Fact]
        public void InvokeGenericMethodRecursively_inNextLayer()
        {
            var actual = guineaPig.InvokeGenericMethodRecursively("Generic", new Type[] { typeof(Attribute) }, "hello from", "the base of");

            Assert.Equal("hello from the base of Attribute", actual);
        }

        #region GuineaPig
        private class GuineaPig : GuineaPigBase
        {
            public void OneParam(string a) { }

            public string OneParamWithReturn(string a) { return a; }

            public void ZeroParams() { }

            public int ZeroParamsWithReturn() { return 666; }

            public string Generic<T>(string b) { return $"{b} {typeof(T).Name}"; }
        }

        private abstract class GuineaPigBase
        {
            public string Generic<T>(string b, string c) { return $"{b} {c} {typeof(T).Name}"; }
        }
        #endregion
    }
}