using System;
using System.Collections.Generic;
using FakeItEasy;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Reflection.InstanceTraverserTests
{
    public class InstanceTraverser_DictionaryTests : InstanceTraverserTestBase
    {
        [Fact]
        public void Dictionary_OnProperty_stopTraversing_becauseOnPropertyReturnsFalse()
        {
            var victim = new SimpleKeyValueVictim
            {
                Dictionary = new Dictionary<string, int>
                {
                    {"one", 1},
                    {"two", 2}
                }
            };

            var type = victim.GetType();
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"Dictionary[one]", 0},
                {"Dictionary[two]", 0}
            };
            
            
            // the calls for the list field and each of its elements
            A.CallTo(() => listener.OnProperty(GetProp(type, "Dictionary"), A<Func<object>>._, A<InstanceTraversalContext>._))
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
                })
                .Returns(false); // do not traverse further down this path
            
            Traverser.TraverseInstance(victim, 5, listener);
            
            Assert.Equal(1, callCounts[".Root"]);
            
            Assert.Equal(0, callCounts["Dictionary[one]"]);
            Assert.Equal(0, callCounts["Dictionary[two]"]);
        }
        
        [Fact]
        public void Dictionary_OnProperty_simpleKey_simpleValue()
        {
            var victim = new SimpleKeyValueVictim
            {
                Dictionary = new Dictionary<string, int>
                {
                    {"one", 1},
                    {"two", 2}
                }
            };

            var type = victim.GetType();
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"Dictionary[one]", 0},
                {"Dictionary[two]", 0}
            };
            
            // the calls for the list field and each of its elements
            A.CallTo(() => listener.OnProperty(GetProp(type, "Dictionary"), A<Func<object>>._, A<InstanceTraversalContext>._))
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
                })
                .Returns(true);
            
            Traverser.TraverseInstance(victim, 5, listener);
            
            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        
        [Fact]
        public void Dictionary_OnProperty_simpleKey_complexValue()
        {
            var victim = new SimpleKeyComplexValueVictim
            {
                Dictionary = new Dictionary<string, ComplexValue>
                {
                    {"one", new ComplexValue()},
                    {"two", new ComplexValue()}
                }
            };

            var type = victim.GetType();
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"Dictionary[one]", 0},
                {"Dictionary[one].String", 0},
                {"Dictionary[two]", 0},
                {"Dictionary[two].String", 0}
            };
            
            // the calls for the list field and each of its elements
            A.CallTo(() => listener.OnProperty(GetProp(type, "Dictionary"), A<Func<object>>._, A<InstanceTraversalContext>._))
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
                })
                .Returns(true);

            A.CallTo(() => listener.OnProperty(GetProp(typeof(ComplexValue), "String"), A<Func<object>>._,
                    A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];

                    Assert.Equal(1, ctx.CurrentDepth);
                    callCounts[$"{ctx.BreadcrumbAsString}.String"]++;
                })
                .Returns(true);
            
            Traverser.TraverseInstance(victim, 5, listener);
            
            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        
        [Fact]
        public void Dictionary_OnProperty_complexKey_usesToString()
        {
            var victim = new ComplexKeyVictim
            {
                Dictionary = new Dictionary<ComplexValue, string>
                {
                    {new ComplexValue { String = "one" }, "1"},
                    {new ComplexValue { String = "two" }, "2"}
                }
            };

            var type = victim.GetType();
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"Dictionary[ComplexValueToString_two]", 0},
                {"Dictionary[ComplexValueToString_one]", 0}
            };
            
            // the calls for the list field and each of its elements
            A.CallTo(() => listener.OnProperty(GetProp(type, "Dictionary"), A<Func<object>>._, A<InstanceTraversalContext>._))
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
                })
                .Returns(true);
            
            Traverser.TraverseInstance(victim, 5, listener);
            
            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        
        [Fact]
        public void Dictionary_OnProperty_interfaced_property()
        {
            var victim = new InterfacedVictim
            {
                Dictionary = new Dictionary<string, int>
                {
                    {"one", 1},
                    {"two", 2}
                }
            };

            var type = victim.GetType();
            
            var callCounts = new Dictionary<string, int>
            {
                {".Root", 0},
                {"Dictionary[one]", 0},
                {"Dictionary[two]", 0}
            };
            
            // the calls for the list field and each of its elements
            A.CallTo(() => listener.OnProperty(GetProp(type, "Dictionary"), A<Func<object>>._, A<InstanceTraversalContext>._))
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
                })
                .Returns(true);
            
            Traverser.TraverseInstance(victim, 5, listener);
            
            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        
        #region Victims
        public class SimpleKeyValueVictim
        {
            public Dictionary<string, int> Dictionary { get; set; }
        }
        
        public class SimpleKeyComplexValueVictim
        {
            public Dictionary<string, ComplexValue> Dictionary { get; set; }
        }
        
        public class ComplexKeyVictim
        {
            public Dictionary<ComplexValue, string> Dictionary { get; set; }
        }
        
        public class InterfacedVictim
        {
            public IDictionary<string, int> Dictionary { get; set; }
        }
        
        public class ComplexValue
        {
            public string String { get; set; } = "Hephey";

            public override string ToString()
            {
                return $"ComplexValueToString_{String}";
            }
        }
        #endregion
    }
}