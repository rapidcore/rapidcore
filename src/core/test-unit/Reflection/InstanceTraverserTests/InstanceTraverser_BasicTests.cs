using System;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using FakeItEasy.Core;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Reflection.InstanceTraverserTests
{
    public class InstanceTraverser_BasicTests : InstanceTraverserTestBase
    {
        [Fact]
        public void EventHandlersAreCalled()
        {
            var victim = new Victim();
            
            Traverser.TraverseInstance(victim, 5, listener);

            var type = typeof(Victim);

            A.CallTo(() => listener.OnConstructor(GetConstructor(type, new Type[0]), A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnConstructor(GetConstructor(type, new[] {typeof(string)}), A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            
            A.CallTo(() => listener.OnField(GetField(type, "PrivateField"), A<Func<object>>._, A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnField(GetField(type, "PrivateStaticField"), A<Func<object>>._, A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnField(GetField(type, "PublicField"), A<Func<object>>._, A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnField(GetField(type, "BaseField"), A<Func<object>>._, A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            
            A.CallTo(() => listener.OnProperty(GetProp(type, "PrivateProperty"), A<Func<object>>._, A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnProperty(GetProp(type, "PrivateStaticProperty"), A<Func<object>>._, A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnProperty(GetProp(type, "PublicProperty"), A<Func<object>>._, A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnProperty(GetProp(type, "BaseProperty"), A<Func<object>>._, A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            
            A.CallTo(() => listener.OnMethod(GetMethod(type, "PrivateMethod", new Type[0]), A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnMethod(GetMethod(type, "PrivateStaticMethod", new Type[0]), A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnMethod(GetMethod(type, "PublicMethod", new Type[0]), A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnMethod(GetMethod(type, "BaseMethod", new Type[0]), A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnMethod(GetMethod(type, "OverloadedMethod", new Type[0]), A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnMethod(GetMethod(type, "OverloadedMethod", new [] { typeof(int) }), A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => listener.OnMethod(GetMethod(type, "OverloadedMethod", new [] { typeof(int), typeof(int) }), A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
            
            A.CallTo(() => listener.OnMaxDepthReached(A<InstanceTraversalContext>._)).MustNotHaveHappened();
        }
        
        [Fact]
        public void EventHandlersAreCalled_withCorrect_context()
        {
            var victim = new Victim();
            
            Action<IFakeObjectCall> invocation = fakeObjectCall =>
            {
                var context = (InstanceTraversalContext)fakeObjectCall.Arguments.Last();
                Assert.Same(victim, context.Instance);
                Assert.Equal(0, context.CurrentDepth);
                Assert.Equal(5, context.MaxDepth);
            };

            A.CallTo(() => listener.OnConstructor(A<ConstructorInfo>._, A<InstanceTraversalContext>._)).Invokes(invocation);
            A.CallTo(() => listener.OnField(A<FieldInfo>._, A<Func<object>>._, A<InstanceTraversalContext>._)).Invokes(invocation);
            A.CallTo(() => listener.OnProperty(A<PropertyInfo>._, A<Func<object>>._, A<InstanceTraversalContext>._)).Invokes(invocation);
            A.CallTo(() => listener.OnMethod(A<MethodInfo>._, A<InstanceTraversalContext>._)).Invokes(invocation);
            
            Traverser.TraverseInstance(victim, 5, listener);
        }
        
        [Fact]
        public void EventHandlersAreCalled_withCorrect_valueGetter()
        {
            var victim = new Victim();

            Action<IFakeObjectCall> fieldInvocation = fakeObjectCall =>
            {
                var valueGetter = (Func<object>) fakeObjectCall.Arguments[1];
                Assert.Contains(" field", valueGetter.Invoke().ToString());
            };
            
            Action<IFakeObjectCall> propertyInvocation = fakeObjectCall =>
            {
                var valueGetter = (Func<object>) fakeObjectCall.Arguments[1];
                Assert.Contains(" property", valueGetter.Invoke().ToString());
            };

            A.CallTo(() => listener.OnField(A<FieldInfo>._, A<Func<object>>._, A<InstanceTraversalContext>._)).Invokes(fieldInvocation);
            A.CallTo(() => listener.OnProperty(A<PropertyInfo>._, A<Func<object>>._, A<InstanceTraversalContext>._)).Invokes(propertyInvocation);
            
            Traverser.TraverseInstance(victim, 5, listener);
        }

        [Fact]
        public void IgnoreMembersDefinedOn_theLowLevel_Object_type()
        {
            var victim = new Victim();
            
            Traverser.TraverseInstance(victim, 5, listener);
            
            // these are just a couple of examples
            A.CallTo(() => listener.OnMethod(A<MethodInfo>.That.Matches(x => x.Name.Equals("ToString")), A<InstanceTraversalContext>._)).MustNotHaveHappened();
            A.CallTo(() => listener.OnMethod(A<MethodInfo>.That.Matches(x => x.Name.Equals("GetHashCode")), A<InstanceTraversalContext>._)).MustNotHaveHappened();
        }
        
        [Fact]
        public void IgnoreAutoProperty_backing_fields()
        {
            var victim = new Victim();
            
            Traverser.TraverseInstance(victim, 5, listener);
            
            // this is just one example
            A.CallTo(() => listener.OnField(A<FieldInfo>.That.Matches(x => x.Name.Equals("<PublicProperty>k__BackingField")), A<Func<object>>._, A<InstanceTraversalContext>._)).MustNotHaveHappened();
        }
        
        [Fact]
        public void IgnoreAutoProperty_backing_methods()
        {
            var victim = new Victim();
            
            Traverser.TraverseInstance(victim, 5, listener);
            
            // this is just one example
            A.CallTo(() => listener.OnMethod(A<MethodInfo>.That.Matches(x => x.Name.Equals("<PublicProperty>k__BackingField")), A<InstanceTraversalContext>._)).MustNotHaveHappened();
        }

        #region Victims
        public class Victim : VictimBase
        {
            public Victim() { }
            
            public Victim(string x) { }

            private static string PrivateStaticField = "private static field";
            
            private string PrivateField = "private field";

            public string PublicField = "public field";

            private string PrivateProperty { get; set; } = "private property";
            
            private static string PrivateStaticProperty { get; set; } = "private static property";

            public string PublicProperty { get; set; } = "public property";
            
            private void PrivateMethod() { }
            
            private static void PrivateStaticMethod() { }
            
            public void PublicMethod() { }
            
            public void OverloadedMethod() { }
            public void OverloadedMethod(int x) { }
            public void OverloadedMethod(int x, int y) { }
        }

        public abstract class VictimBase
        {
            protected VictimBase() { } // this does not come out
            public string BaseField = "base field";
            public string BaseProperty { get; set; } = "base property";

            public string BaseMethod()
            {
                return "x";
            }
        }
        #endregion
    }
}