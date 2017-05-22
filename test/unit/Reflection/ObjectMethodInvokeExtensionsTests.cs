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
        public void InvokeMethodRecursively_CanInvoke_NullParams_lastOne()
        {
            guineaPig.InvokeMethodRecursively("MultipleParams", "one", "two", null);
        }

        [Fact]
        public void InvokeMethodRecursively_CanInvoke_NullParams_notFirstNotLast()
        {
            guineaPig.InvokeMethodRecursively("MultipleParams", "one", null, "three");
        }

        [Fact]
        public void InvokeMethodRecursively_CanInvoke_NullParams_firstOne()
        {
            guineaPig.InvokeMethodRecursively("MultipleParams", null, "two", "three");
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

        [Fact]
        public void InvokeGetterRecursively_GetterAndSetter_inFirstLayer()
        {
            guineaPig.GetterAndSetter = "yo";

            var actual = guineaPig.InvokeGetterRecursively("GetterAndSetter");

            Assert.Equal("yo", actual);
        }

        [Fact]
        public void InvokeGetterRecursively_GetterAndSetter_inNextLayer()
        {
            guineaPig.AllAboutThatBase = "aahhhhh yeah";

            var actual = guineaPig.InvokeGetterRecursively("AllAboutThatBase");

            Assert.Equal("aahhhhh yeah", actual);
        }

        [Fact]
        public void InvokeGetterRecursively_Getter_inFirstLayer()
        {
            var actual = guineaPig.InvokeGetterRecursively("Getter");

            Assert.Equal("getter only", actual);
        }

        [Fact]
        public void InvokeGetterRecursively_Throws_ifThereIsNoGetter()
        {

            var ex = Assert.Throws<MissingMethodException>(() => guineaPig.InvokeGetterRecursively("SetterOnly"));

            Assert.Equal("The property SetterOnly does not have a getter", ex.Message);
        }

        [Fact]
        public void InvokeGetterRecursively_Throws_ifThePropertyDoesNotExist()
        {
            Assert.ThrowsAny<MissingMemberException>(() => guineaPig.InvokeGetterRecursively("DoesNotExist"));
        }

        [Fact]
        public void InvokeSetterRecursively_GetterAndSetter_inFirstLayer()
        {
            guineaPig.InvokeSetterRecursively("GetterAndSetter", "hephey");

            Assert.Equal("hephey", guineaPig.GetterAndSetter);
        }

        [Fact]
        public void InvokeSetterRecursively_GetterAndSetter_inNextLayer()
        {
            guineaPig.InvokeSetterRecursively("AllAboutThatBase", "hephey");

            Assert.Equal("hephey", guineaPig.AllAboutThatBase);
        }

        [Fact]
        public void InvokeSetterRecursively_Throws_ifThereIsNoSetter()
        {

            var ex = Assert.Throws<MissingMethodException>(() => guineaPig.InvokeSetterRecursively("Getter", "!"));

            Assert.Equal("The property Getter does not have a setter", ex.Message);
        }

        [Fact]
        public void InvokeSetterRecursively_Throws_ifThePropertyDoesNotExist()
        {
            Assert.ThrowsAny<MissingMemberException>(() => guineaPig.InvokeSetterRecursively("DoesNotExist", "!"));
        }

        #region GuineaPig
        private class GuineaPig : GuineaPigBase
        {
            public void OneParam(string a) { }

            public string OneParamWithReturn(string a) { return a; }

            public void ZeroParams() { }

            public int ZeroParamsWithReturn() { return 666; }

            public string Generic<T>(string b) { return $"{b} {typeof(T).Name}"; }

            public string GetterAndSetter { get; set; }

            public string Getter => "getter only";

            private string setterOnly;

            public string SetterOnly
            {
                set
                {
                    setterOnly = value;
                }
            }
        }

        private abstract class GuineaPigBase
        {
            public string Generic<T>(string b, string c) { return $"{b} {c} {typeof(T).Name}"; }

            public string AllAboutThatBase { get; set; }

            public void MultipleParams(string one, string two, string three) { }
        }
        #endregion
    }
}