using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FakeItEasy;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Reflection.InstanceTraverserTests
{
    public class InstanceTraverser_EnumerableTests : InstanceTraverserTestBase
    {
        #region List with elements
        [Fact]
        public void List_withEqualSizeAndCapacity_isDirectlyPassedTo_TraverseInstance_OnField_isCalled_forEachElement_andRecurses()
        {
            var victim = new List<InnocentBystander>
            {
                new InnocentBystander(),
                new InnocentBystander()
            };
            victim.Capacity = 2;

            var type = victim.GetType();
            var complexType = typeof(InnocentBystander);
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"_items[0]", 0},
                {"_items[0].FieldString", 0},
                {"_items[1]", 0},
                {"_items[1].FieldString", 0}
            };
            
            A.CallTo(() =>
                    listener.OnField(GetField(type, "_items"), A<Func<object>>._, A<IReadOnlyInstanceTraversalContext>._))
                .Invokes(
                    x =>
                    {
                        var ctx = (InstanceTraversalContext) x.Arguments[2];
                        
                        if (ctx.CurrentDepth == 0)
                        {
                            callCounts[".Root"]++;
                            return;
                        }
                        
                        // for the individual elements
                        Assert.Equal(1, ctx.CurrentDepth);
                        callCounts[ctx.BreadcrumbAsString]++;
                    });
            
            
            A.CallTo(() =>
                    listener.OnField(GetField(complexType, "FieldString"), A<Func<object>>._, A<IReadOnlyInstanceTraversalContext>._))
                .Invokes(
                    x =>
                    {
                        var ctx = (InstanceTraversalContext) x.Arguments[2];
                        
                        Assert.Equal(1, ctx.CurrentDepth);
                        callCounts[$"{ctx.BreadcrumbAsString}.FieldString"]++;
                    });

            
            Traverser.TraverseInstance(victim, 10, listener);
            
            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        #endregion
        
        #region Field with array
        [Fact]
        public void Array_OnField_isCalled_forEachElement_simpleType()
        {
            var victim = new ArrayVictim {FieldInts = new[] {11, 22, 33}};
            
            var type = victim.GetType();
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"FieldInts[0]", 0},
                {"FieldInts[1]", 0},
                {"FieldInts[2]", 0}
            };
            
            A.CallTo(() => listener.OnField(GetField(type, "FieldInts"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];

                    if (ctx.CurrentDepth == 0)
                    {
                        callCounts[".Root"]++;
                        return;
                    }
                    
                    // for the individual elements
                    Assert.Equal(1, ctx.CurrentDepth);
                    callCounts[ctx.BreadcrumbAsString]++;
                });
            
            Traverser.TraverseInstance(victim, 5, listener);

            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        
        [Fact]
        public void Array_OnField_isCalled_forEachElement_andRecurses()
        {
            var one = new InnocentBystander();
            var two = new InnocentBystander();
            var three = new InnocentBystander();
            var victim = new ArrayVictim {FieldComplex = new[]{one, two, three}};
            
            var type = victim.GetType();
            var complexType = typeof(InnocentBystander);
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"FieldComplex[0]", 0},
                {"FieldComplex[0].FieldString", 0},
                {"FieldComplex[1]", 0},
                {"FieldComplex[1].FieldString", 0},
                {"FieldComplex[2]", 0},
                {"FieldComplex[2].FieldString", 0}
            };
            
            // the calls for the array field and each of its elements
            A.CallTo(() => listener.OnField(GetField(type, "FieldComplex"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];

                    if (ctx.CurrentDepth == 0)
                    {
                        callCounts[".Root"]++;
                        return;
                    }
                    
                    // for the individual elements
                    Assert.Equal(1, ctx.CurrentDepth);
                    callCounts[ctx.BreadcrumbAsString]++;
                });
            
            // the calls for the recursion of each array element
            A.CallTo(() => listener.OnField(GetField(complexType, "FieldString"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    
                    Assert.Equal(1, ctx.CurrentDepth);
                    callCounts[$"{ctx.BreadcrumbAsString}.FieldString"]++;
                });
            
            Traverser.TraverseInstance(victim, 5, listener);

            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        #endregion
        
        #region Property with array
        [Fact]
        public void Array_OnProperty_isCalled_forEachElement_simpleType_andDoesNotRecurse()
        {
            var victim = new ArrayVictim {PropInts = new[] {11, 22, 33}};
            
            var type = victim.GetType();
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"PropInts[0]", 0},
                {"PropInts[1]", 0},
                {"PropInts[2]", 0}
            };
            
            A.CallTo(() => listener.OnProperty(GetProp(type, "PropInts"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];

                    if (ctx.CurrentDepth == 0)
                    {
                        callCounts[".Root"]++;
                        return;
                    }
                    
                    // for the individual elements
                    Assert.Equal(1, ctx.CurrentDepth);
                    callCounts[ctx.BreadcrumbAsString]++;
                });
            
            Traverser.TraverseInstance(victim, 5, listener);
            
            A.CallTo(() => listener.OnField(A<FieldInfo>.That.Matches(x => x.Name.Equals("m_value")), A<Func<object>>._, A<IReadOnlyInstanceTraversalContext>._)).MustNotHaveHappened();

            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        
        [Fact]
        public void Array_OnProperty_isCalled_forEachElement_andRecurses()
        {
            var one = new InnocentBystander();
            var two = new InnocentBystander();
            var three = new InnocentBystander();
            var victim = new ArrayVictim {PropComplex = new[]{one, two, three}};
            
            var type = victim.GetType();
            var complexType = typeof(InnocentBystander);
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"PropComplex[0]", 0},
                {"PropComplex[0].FieldString", 0},
                {"PropComplex[1]", 0},
                {"PropComplex[1].FieldString", 0},
                {"PropComplex[2]", 0},
                {"PropComplex[2].FieldString", 0}
            };
            
            // the calls for the array Prop and each of its elements
            A.CallTo(() => listener.OnProperty(GetProp(type, "PropComplex"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];

                    if (ctx.CurrentDepth == 0)
                    {
                        callCounts[".Root"]++;
                        return;
                    }
                    
                    // for the individual elements
                    Assert.Equal(1, ctx.CurrentDepth);
                    callCounts[ctx.BreadcrumbAsString]++;
                });
            
            // the calls for the recursion of each array element
            A.CallTo(() => listener.OnField(GetField(complexType, "FieldString"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    
                    Assert.Equal(1, ctx.CurrentDepth);
                    callCounts[$"{ctx.BreadcrumbAsString}.FieldString"]++;
                });
            
            Traverser.TraverseInstance(victim, 5, listener);

            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        #endregion
        
        #region Field with list
        [Fact]
        public void List_OnField_isCalled_forEachElement_andRecurses()
        {
            var one = new InnocentBystander();
            var two = new InnocentBystander();
            var three = new InnocentBystander();
            var victim = new ListVictim {FieldList = new List<InnocentBystander> {one, two, three}};
            
            var type = victim.GetType();
            var complexType = typeof(InnocentBystander);
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"FieldList[0]", 0},
                {"FieldList[0].FieldString", 0},
                {"FieldList[1]", 0},
                {"FieldList[1].FieldString", 0},
                {"FieldList[2]", 0},
                {"FieldList[2].FieldString", 0}
            };
            
            // the calls for the list field and each of its elements
            A.CallTo(() => listener.OnField(GetField(type, "FieldList"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];

                    if (ctx.CurrentDepth == 0)
                    {
                        callCounts[".Root"]++;
                        return;
                    }
                    
                    // for the individual elements
                    Assert.Equal(1, ctx.CurrentDepth);
                    callCounts[ctx.BreadcrumbAsString]++;
                });
            
            // the calls for the recursion of each list element
            A.CallTo(() => listener.OnField(GetField(complexType, "FieldString"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    
                    Assert.Equal(1, ctx.CurrentDepth);
                    callCounts[$"{ctx.BreadcrumbAsString}.FieldString"]++;
                });
            
            Traverser.TraverseInstance(victim, 5, listener);

            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        #endregion
        
        #region Property with list
        [Fact]
        public void List_OnProperty_isCalled_forEachElement_andRecurses()
        {
            var one = new InnocentBystander();
            var two = new InnocentBystander();
            var three = new InnocentBystander();
            var victim = new ListVictim {PropList = new List<InnocentBystander> {one, two, three}};
            
            var type = victim.GetType();
            var complexType = typeof(InnocentBystander);
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"PropList[0]", 0},
                {"PropList[0].FieldString", 0},
                {"PropList[1]", 0},
                {"PropList[1].FieldString", 0},
                {"PropList[2]", 0},
                {"PropList[2].FieldString", 0}
            };
            
            // the calls for the list field and each of its elements
            A.CallTo(() => listener.OnProperty(GetProp(type, "PropList"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];

                    if (ctx.CurrentDepth == 0)
                    {
                        callCounts[".Root"]++;
                        return;
                    }
                    
                    // for the individual elements
                    Assert.Equal(1, ctx.CurrentDepth);
                    callCounts[ctx.BreadcrumbAsString]++;
                });
            
            // the calls for the recursion of each list element
            A.CallTo(() => listener.OnField(GetField(complexType, "FieldString"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    
                    Assert.Equal(1, ctx.CurrentDepth);
                    callCounts[$"{ctx.BreadcrumbAsString}.FieldString"]++;
                });
            
            Traverser.TraverseInstance(victim, 5, listener);

            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        
        [Fact]
        public void List_OnProperty_isCalled_forEachElement_but_doesNotRecurse_simple_types()
        {
            var victim = new ListVictim {PropIntList = new List<int> {11, 22}};
            
            var type = victim.GetType();
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"PropIntList[0]", 0},
                {"PropIntList[1]", 0}
            };
            
            // the calls for the list field and each of its elements
            A.CallTo(() => listener.OnProperty(GetProp(type, "PropIntList"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];

                    if (ctx.CurrentDepth == 0)
                    {
                        callCounts[".Root"]++;
                        return;
                    }
                    
                    // for the individual elements
                    Assert.Equal(1, ctx.CurrentDepth);
                    callCounts[ctx.BreadcrumbAsString]++;
                });
            
            Traverser.TraverseInstance(victim, 5, listener);
            
            A.CallTo(() => listener.OnField(
                A<FieldInfo>.That.Not.Matches(x => x == GetField(type, "FieldList")), 
                A<Func<object>>._, 
                A<IReadOnlyInstanceTraversalContext>._)
            ).MustNotHaveHappened();

            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        #endregion

        #region Victims
        public class InnocentBystander
        {
            public string FieldString = "what is that nois...";
        }
        
        public class ArrayVictim
        {
            public int[] FieldInts = null;
            public int[] PropInts { get; set; } = null;

            public InnocentBystander[] FieldComplex = null;
            public InnocentBystander[] PropComplex { get; set; } = null;
        }
        
        public class ListVictim
        {
            public List<InnocentBystander> FieldList = null;
            public List<InnocentBystander> PropList { get; set; } = null;
            public IEnumerable PropIntList { get; set; } = null;
        }
        #endregion
    }
}