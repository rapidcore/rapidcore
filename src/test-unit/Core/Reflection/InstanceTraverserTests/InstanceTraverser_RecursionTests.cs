using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FakeItEasy;
using RapidCore.Reflection;
using Xunit;

namespace UnitTests.Core.Reflection.InstanceTraverserTests
{
    public class InstanceTraverser_RecursionTests : InstanceTraverserTestBase
    {
        [Fact]
        public void RecursionDoesNotCall_OnConstructor_forRecursedTypes()
        {
            var victim = new RecursionVictim();
            var childTwo = new RecursionChildTwo();
            var childThree = new RecursionChildThree
            {
                ChildTwo = childTwo
            };

            victim.PropChildTwo = childTwo;
            victim.PropChildThree = childThree;
            
            Traverser.TraverseInstance(victim, 5, listener);

            var victimCtor = GetConstructor(typeof(RecursionVictim), new Type[0]);
            
            A.CallTo(() => listener.OnConstructor(A<ConstructorInfo>.That.Matches(x => x != victimCtor), A<InstanceTraversalContext>._)).MustNotHaveHappened();
        }

        [Fact]
        public void RecursionDoesNotHappenOnPrimitives_nullables_andCertain_builtIns()
        {
            var victim = new NoRecursionOnTheseTypesVictim();
            
            Traverser.TraverseInstance(victim, 5, listener);
            
            A.CallTo(() => listener.OnField(A<FieldInfo>._, A<Func<object>>._, A<InstanceTraversalContext>.That.Matches(x => x.CurrentDepth > 0))).MustNotHaveHappened();
            A.CallTo(() => listener.OnProperty(A<PropertyInfo>._, A<Func<object>>._, A<InstanceTraversalContext>.That.Matches(x => x.CurrentDepth > 0))).MustNotHaveHappened();
            A.CallTo(() => listener.OnMethod(A<MethodInfo>._, A<InstanceTraversalContext>._)).MustNotHaveHappened();
        }
        
        #region Fields

        [Fact]
        public void FieldRecursion_stopsWhenOnFieldReturnsFalse()
        {
            var victim = new RecursionVictim();
            var childTwo = new RecursionChildTwo();
            var childThree = new RecursionChildThree
            {
                ChildTwo = childTwo
            };

            victim.FieldChildTwo = childTwo;
            victim.FieldChildThree = childThree;

            var type = victim.GetType();
            var child2Type = typeof(RecursionChildTwo);
            var child3Type = typeof(RecursionChildThree);


            var callCounts = new Dictionary<string, int>
            {
                {".FieldChildTwo", 0},
                {"FieldChildTwo.ChildTwoString", 0},
                {".FieldChildThree", 0},
                {"FieldChildThree.ChildThreeString", 0},
                {"FieldChildThree.ChildTwo", 0},
                {"FieldChildThree.ChildTwo.ChildTwoString", 0}
            };
            
            // first we get a call for the top level child two field
            A.CallTo(() => listener.OnField(GetField(type, "FieldChildTwo"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(0, ctx.CurrentDepth);

                    callCounts[".FieldChildTwo"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            // we then dig into the top level child two field instance
            // and this is also triggered for the FieldChildThree.ChildTwo instance
            A.CallTo(() => listener.OnProperty(GetProp(child2Type, "ChildTwoString"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];

                    if (ctx.CurrentDepth == 1)
                    {
                        Assert.Equal(1, ctx.CurrentDepth);
                        Assert.Equal("FieldChildTwo", ctx.BreadcrumbAsString);
                    
                        callCounts["FieldChildTwo.ChildTwoString"]++;
                    }
                    else if (ctx.CurrentDepth == 2)
                    {
                        Assert.Equal(2, ctx.CurrentDepth);
                        Assert.Equal("FieldChildThree.ChildTwo", ctx.BreadcrumbAsString);
                    
                        callCounts["FieldChildThree.ChildTwo.ChildTwoString"]++;
                    }
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            // then we visit the FieldChildThree top level field
            A.CallTo(() => listener.OnField(GetField(type, "FieldChildThree"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(0, ctx.CurrentDepth);
                    
                    callCounts[".FieldChildThree"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = false }); // do not traverse further down this path
            
            
            // if traversal continues although it should not, then FieldChildThree.ChildThreeString is next
            A.CallTo(() => listener.OnProperty(GetProp(child3Type, "ChildThreeString"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(1, ctx.CurrentDepth);
                    Assert.Equal("FieldChildThree", ctx.BreadcrumbAsString);
                    
                    callCounts["FieldChildThree.ChildThreeString"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            
            // if traversal continues although it should not, then FieldChildThree.ChildTwo is next
            A.CallTo(() => listener.OnProperty(GetProp(child3Type, "ChildTwo"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(1, ctx.CurrentDepth);
                    Assert.Equal("FieldChildThree", ctx.BreadcrumbAsString);
                    
                    callCounts["FieldChildThree.ChildTwo"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
                
            Traverser.TraverseInstance(victim, 5, listener);
            
            // these should have been called exactly once
            Assert.Equal(1, callCounts[".FieldChildTwo"]);
            Assert.Equal(1, callCounts["FieldChildTwo.ChildTwoString"]);
            Assert.Equal(1, callCounts[".FieldChildThree"]);

            // these should not have been called as traversal should stop at .FieldChildThree
            Assert.Equal(0, callCounts["FieldChildThree.ChildThreeString"]);
            Assert.Equal(0, callCounts["FieldChildThree.ChildTwo"]);
            Assert.Equal(0, callCounts["FieldChildThree.ChildTwo.ChildTwoString"]);
        }
        
        [Fact]
        public void FieldRecursion_withinDepth_works()
        {
            var victim = new RecursionVictim();
            var childTwo = new RecursionChildTwo();
            var childThree = new RecursionChildThree
            {
                ChildTwo = childTwo
            };

            victim.FieldChildTwo = childTwo;
            victim.FieldChildThree = childThree;

            var type = victim.GetType();
            var child2Type = typeof(RecursionChildTwo);
            var child3Type = typeof(RecursionChildThree);


            var callCounts = new Dictionary<string, int>
            {
                {".FieldChildTwo", 0},
                {"FieldChildTwo.ChildTwoString", 0},
                {".FieldChildThree", 0},
                {"FieldChildThree.ChildThreeString", 0},
                {"FieldChildThree.ChildTwo", 0},
                {"FieldChildThree.ChildTwo.ChildTwoString", 0}
            };
            
            // first we get a call for the top level child two field
            A.CallTo(() => listener.OnField(GetField(type, "FieldChildTwo"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(0, ctx.CurrentDepth);

                    callCounts[".FieldChildTwo"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            // we then dig into the top level child two field instance
            // and this is also triggered for the FieldChildThree.ChildTwo instance
            A.CallTo(() => listener.OnProperty(GetProp(child2Type, "ChildTwoString"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];

                    if (ctx.CurrentDepth == 1)
                    {
                        Assert.Equal(1, ctx.CurrentDepth);
                        Assert.Equal("FieldChildTwo", ctx.BreadcrumbAsString);
                    
                        callCounts["FieldChildTwo.ChildTwoString"]++;
                    }
                    else if (ctx.CurrentDepth == 2)
                    {
                        Assert.Equal(2, ctx.CurrentDepth);
                        Assert.Equal("FieldChildThree.ChildTwo", ctx.BreadcrumbAsString);
                    
                        callCounts["FieldChildThree.ChildTwo.ChildTwoString"]++;
                    }
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            // then we visit the FieldChildThree top level field
            A.CallTo(() => listener.OnField(GetField(type, "FieldChildThree"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(0, ctx.CurrentDepth);
                    
                    callCounts[".FieldChildThree"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            
            // next up is FieldChildThree.ChildThreeString
            A.CallTo(() => listener.OnProperty(GetProp(child3Type, "ChildThreeString"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(1, ctx.CurrentDepth);
                    Assert.Equal("FieldChildThree", ctx.BreadcrumbAsString);
                    
                    callCounts["FieldChildThree.ChildThreeString"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            
            // next up is FieldChildThree.ChildTwo
            A.CallTo(() => listener.OnProperty(GetProp(child3Type, "ChildTwo"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(1, ctx.CurrentDepth);
                    Assert.Equal("FieldChildThree", ctx.BreadcrumbAsString);
                    
                    callCounts["FieldChildThree.ChildTwo"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
                
            Traverser.TraverseInstance(victim, 5, listener);
            
            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        
        [Fact]
        public void FieldRecursion_callsOnMaxDepthReached()
        {
            var victim = new RecursionVictim();
            var childTwo = new RecursionChildTwo();
            var childThree = new RecursionChildThree
            {
                ChildTwo = childTwo
            };

            victim.FieldChildTwo = childTwo;
            victim.FieldChildThree = childThree;

            Traverser.TraverseInstance(victim, 1, listener);

            A.CallTo(() => listener.OnMaxDepthReached(A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
        }
        #endregion
        
        #region Properties
        [Fact]
        public void PropertyRecursion_stopsWhenOnPropertyReturnsFalse()
        {
            var victim = new RecursionVictim();
            var childTwo = new RecursionChildTwo();
            var childThree = new RecursionChildThree
            {
                ChildTwo = childTwo
            };

            victim.PropChildTwo = childTwo;
            victim.PropChildThree = childThree;

            var type = victim.GetType();
            var child2Type = typeof(RecursionChildTwo);
            var child3Type = typeof(RecursionChildThree);


            var callCounts = new Dictionary<string, int>
            {
                {".PropChildTwo", 0},
                {"PropChildTwo.ChildTwoString", 0},
                {".PropChildThree", 0},
                {"PropChildThree.ChildThreeString", 0},
                {"PropChildThree.ChildTwo", 0},
                {"PropChildThree.ChildTwo.ChildTwoString", 0}
            };
            
            // first we get a call for the top level child two property
            A.CallTo(() => listener.OnProperty(GetProp(type, "PropChildTwo"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(0, ctx.CurrentDepth);

                    callCounts[".PropChildTwo"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            // we then dig into the top level child two prop instance
            // and this is also triggered for the PropChildThree.ChildTwo instance
            A.CallTo(() => listener.OnProperty(GetProp(child2Type, "ChildTwoString"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];

                    if (ctx.CurrentDepth == 1)
                    {
                        Assert.Equal(1, ctx.CurrentDepth);
                        Assert.Equal("PropChildTwo", ctx.BreadcrumbAsString);
                    
                        callCounts["PropChildTwo.ChildTwoString"]++;
                    }
                    else if (ctx.CurrentDepth == 2)
                    {
                        Assert.Equal(2, ctx.CurrentDepth);
                        Assert.Equal("PropChildThree.ChildTwo", ctx.BreadcrumbAsString);
                    
                        callCounts["PropChildThree.ChildTwo.ChildTwoString"]++;
                    }
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            // then we visit the PropChildThree top level prop
            A.CallTo(() => listener.OnProperty(GetProp(type, "PropChildThree"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(0, ctx.CurrentDepth);
                    
                    callCounts[".PropChildThree"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = false }); // do not traverse further down this path
            
            
            // if traversal continues although it should not, then PropChildThree.ChildThreeString is next
            A.CallTo(() => listener.OnProperty(GetProp(child3Type, "ChildThreeString"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(1, ctx.CurrentDepth);
                    Assert.Equal("PropChildThree", ctx.BreadcrumbAsString);
                    
                    callCounts["PropChildThree.ChildThreeString"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            
            // if traversal continues although it should not, then PropChildThree.ChildTwo is next
            A.CallTo(() => listener.OnProperty(GetProp(child3Type, "ChildTwo"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(1, ctx.CurrentDepth);
                    Assert.Equal("PropChildThree", ctx.BreadcrumbAsString);
                    
                    callCounts["PropChildThree.ChildTwo"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
                
            Traverser.TraverseInstance(victim, 5, listener);
            
            // these should have been called exactly once
            Assert.Equal(1, callCounts[".PropChildTwo"]);
            Assert.Equal(1, callCounts["PropChildTwo.ChildTwoString"]);
            Assert.Equal(1, callCounts[".PropChildThree"]);

            // these should not have been called as traversal should stop at .PropChildThree
            Assert.Equal(0, callCounts["PropChildThree.ChildThreeString"]);
            Assert.Equal(0, callCounts["PropChildThree.ChildTwo"]);
            Assert.Equal(0, callCounts["PropChildThree.ChildTwo.ChildTwoString"]);
        }
        
        [Fact]
        public void PropertyRecursion_withinDepth_works()
        {
            var victim = new RecursionVictim();
            var childTwo = new RecursionChildTwo();
            var childThree = new RecursionChildThree
            {
                ChildTwo = childTwo
            };

            victim.PropChildTwo = childTwo;
            victim.PropChildThree = childThree;

            var type = victim.GetType();
            var child2Type = typeof(RecursionChildTwo);
            var child3Type = typeof(RecursionChildThree);


            var callCounts = new Dictionary<string, int>
            {
                {".PropChildTwo", 0},
                {"PropChildTwo.ChildTwoString", 0},
                {".PropChildThree", 0},
                {"PropChildThree.ChildThreeString", 0},
                {"PropChildThree.ChildTwo", 0},
                {"PropChildThree.ChildTwo.ChildTwoString", 0}
            };
            
            // first we get a call for the top level child two property
            A.CallTo(() => listener.OnProperty(GetProp(type, "PropChildTwo"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(0, ctx.CurrentDepth);

                    callCounts[".PropChildTwo"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            // we then dig into the top level child two prop instance
            // and this is also triggered for the PropChildThree.ChildTwo instance
            A.CallTo(() => listener.OnProperty(GetProp(child2Type, "ChildTwoString"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];

                    if (ctx.CurrentDepth == 1)
                    {
                        Assert.Equal(1, ctx.CurrentDepth);
                        Assert.Equal("PropChildTwo", ctx.BreadcrumbAsString);
                    
                        callCounts["PropChildTwo.ChildTwoString"]++;
                    }
                    else if (ctx.CurrentDepth == 2)
                    {
                        Assert.Equal(2, ctx.CurrentDepth);
                        Assert.Equal("PropChildThree.ChildTwo", ctx.BreadcrumbAsString);
                    
                        callCounts["PropChildThree.ChildTwo.ChildTwoString"]++;
                    }
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            // then we visit the PropChildThree top level prop
            A.CallTo(() => listener.OnProperty(GetProp(type, "PropChildThree"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(0, ctx.CurrentDepth);
                    
                    callCounts[".PropChildThree"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            
            // next up is PropChildThree.ChildThreeString
            A.CallTo(() => listener.OnProperty(GetProp(child3Type, "ChildThreeString"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(1, ctx.CurrentDepth);
                    Assert.Equal("PropChildThree", ctx.BreadcrumbAsString);
                    
                    callCounts["PropChildThree.ChildThreeString"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
            
            
            // next up is PropChildThree.ChildTwo
            A.CallTo(() => listener.OnProperty(GetProp(child3Type, "ChildTwo"), A<Func<object>>._, A<InstanceTraversalContext>._))
                .Invokes(x =>
                {
                    var ctx = (InstanceTraversalContext) x.Arguments[2];
                    Assert.Equal(1, ctx.CurrentDepth);
                    Assert.Equal("PropChildThree", ctx.BreadcrumbAsString);
                    
                    callCounts["PropChildThree.ChildTwo"]++;
                })
                .Returns(new SimpleInstanceListenerOnFieldOrPropResult { DoContinueRecursion = true });
                
            Traverser.TraverseInstance(victim, 5, listener);
            
            // all "methods" should have been called exactly once
            foreach (var (key, value) in callCounts)
            {
                // this sillyness provides us with a hint for which thing was not called as expected
                Assert.Equal($"{key}=1", $"{key}={value}");
            }
        }
        
        [Fact]
        public void PropertyRecursion_callsOnMaxDepthReached()
        {
            var victim = new RecursionVictim();
            var childTwo = new RecursionChildTwo();
            var childThree = new RecursionChildThree
            {
                ChildTwo = childTwo
            };

            victim.PropChildTwo = childTwo;
            victim.PropChildThree = childThree;

            Traverser.TraverseInstance(victim, 1, listener);

            A.CallTo(() => listener.OnMaxDepthReached(A<InstanceTraversalContext>._)).MustHaveHappenedOnceExactly();
        }
        #endregion

        #region Victims
        public class RecursionVictim
        {
            public RecursionChildTwo FieldChildTwo = null;
            public RecursionChildThree FieldChildThree = null;
            
            public RecursionChildTwo PropChildTwo { get; set; }
            public RecursionChildThree PropChildThree { get; set; }
        }
        
        public class RecursionChildTwo
        {
            public string ChildTwoString { get; set; } = "child two";
        }
        
        public class RecursionChildThree
        {
            public string ChildThreeString { get; set; } = "child three";
            public RecursionChildTwo ChildTwo { get; set; }
        }
        
        public class NoRecursionOnTheseTypesVictim
        {
            // we need the nullables to have values
            // otherwise, recursion will not be done
            // on account of them being null
            
            public char Char => 'c';
            public char? CharNullable => 'c';
            public string String => "hello";
            public bool BoolTrue => true;
            public bool BoolFalse => false;
            public bool? BoolNullable => true;
            public byte Byte => 4;
            public byte? ByteNullable => 4;
            public short Short => 5;
            public short? ShortNullable => 5;
            public int Int => 12345;
            public int? IntNullable => 12345;
            public long Long => 12352326;
            public long? LongNullable => 12352326;
            public float Float => 1.23f;
            public float? FloatNullable => 1.23f;
            public double Double => 123.23;
            public double? DoubleNullable => 123.23;
            public decimal Decimal => 123.467m;
            public decimal? DecimalNullable => 123.467m;
            public DateTime DateTime => DateTime.Now;
            public DateTime? DateTimeNullable => DateTime.Now;
            public DateTimeOffset DateTimeOffset => DateTimeOffset.Now;
            public DateTimeOffset? DateTimeOffsetNullable => DateTimeOffset.Now;
            public TimeSpan TimeSpan => TimeSpan.FromDays(2.1);
            public TimeSpan? TimeSpanNullable => TimeSpan.FromDays(2.1);
            public Guid Guid => Guid.NewGuid();
            public VictimOf Enum => VictimOf.Beauty;
            public VictimOf? EnumNullable => VictimOf.Beauty;
            public MemoryStream Stream => new MemoryStream();
        }
        
        public enum VictimOf
        {
            Beauty,
            Niceness
        }
        #endregion
    }
}