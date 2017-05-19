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

        #region GuineaPig
        private class GuineaPig
        {
            public void OneParam(string a) { }

            public string OneParamWithReturn(string a) { return a; }

            public void ZeroParams() { }

            public int ZeroParamsWithReturn() { return 666; }
        }
        #endregion
    }
}