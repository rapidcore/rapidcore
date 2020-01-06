using System;
using System.Linq;
using RapidCore.Diffing;
using RapidCore.Diffing.Internal;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Diffing.Internal.StateChangeFinderWorkerTests
{
    public class StateChangeFinderWorker_BasicTests
    {
        private readonly StateChangeFinderWorker worker;
        private readonly StateChanges stateChanges;

        public StateChangeFinderWorker_BasicTests()
        {
            stateChanges = new StateChanges();

            worker = new StateChangeFinderWorker(new InstanceTraverser());
        }
        
        [Fact]
        public void FindDifferences_handles_null_new()
        {
            var newState = new TheVictim
            {
                String = "new string"
            };
            
            worker.FindDifferences(null, newState, stateChanges, 5);

            Assert.Equal(1, stateChanges.Changes.Count);
            
            var change = stateChanges.Changes[0];
            Assert.Equal("String", change.Breadcrumb);
            Assert.Null(change.OldValue);
            Assert.Equal("new string", change.NewValue);
        }
        
        [Fact]
        public void FindDifferences_handles_old_null()
        {
            var oldState = new TheVictim
            {
                String = "old string"
            };
            
            worker.FindDifferences(oldState, null, stateChanges, 5);

            Assert.Equal(1, stateChanges.Changes.Count);
            
            var change = stateChanges.Changes[0];
            Assert.Equal("String", change.Breadcrumb);
            Assert.Equal("old string", change.OldValue);
            Assert.Null(change.NewValue);
        }
        
        [Fact]
        public void FindDifferences_works()
        {
            var dt = DateTime.Now;
            var ts = TimeSpan.FromDays(1.234);

            var oldState = new TheVictim
            {
                Bool = true,
                Char = 'c',
                DateTime = dt,
                Decimal = 123.45m,
                Double = 12.34,
                Float = 6.66f,
                Int = 666,
                String = "Kewl string",
                TimeSpan = ts,
                Enum = EnumVictim.One,
                Byte = 5,
                SomeThing = new SomeThing
                {
                    SomeString = "Some"
                },
                InternalIsIncluded = "internal"
            }.SetPrivate("private").SetProtected("protected");
            
            var newState = new TheVictim()
            {
                Bool = false,
                Char = 'x',
                DateTime = dt.AddMinutes(2.1),
                Decimal = 123.50m,
                Double = 12.56,
                Float = 6.78f,
                Int = 777,
                String = "Hot string",
                TimeSpan = ts.Add(TimeSpan.FromMinutes(2.3)),
                Enum = EnumVictim.Two,
                Byte = 9,
                SomeThing = new SomeThing
                {
                    SomeString = "Changed"
                },
                InternalIsIncluded = "changed internal"
            }.SetPrivate("changed").SetProtected("changed protected");

            worker.FindDifferences(oldState, newState, stateChanges, 5);

            Assert.Equal(15, stateChanges.Changes.Count);
            
            var sorted = stateChanges.Changes.OrderBy(x => x.Breadcrumb).ToList();

            var change = sorted[0];
            Assert.Equal("Bool", change.Breadcrumb);
            Assert.Equal(true, change.OldValue);
            Assert.Equal(false, change.NewValue);
            
            change = sorted[1];
            Assert.Equal("Byte", change.Breadcrumb);
            Assert.Equal((byte)5, change.OldValue);
            Assert.Equal((byte)9, change.NewValue);
            
            change = sorted[2];
            Assert.Equal("Char", change.Breadcrumb);
            Assert.Equal('c', change.OldValue);
            Assert.Equal('x', change.NewValue);
            
            change = sorted[3];
            Assert.Equal("DateTime", change.Breadcrumb);
            Assert.Equal(oldState.DateTime, change.OldValue);
            Assert.Equal(newState.DateTime, change.NewValue);
            
            change = sorted[4];
            Assert.Equal("Decimal", change.Breadcrumb);
            Assert.Equal(123.45m, change.OldValue);
            Assert.Equal(123.50m, change.NewValue);
            
            change = sorted[5];
            Assert.Equal("Double", change.Breadcrumb);
            Assert.Equal(12.34, change.OldValue);
            Assert.Equal(12.56, change.NewValue);
            
            change = sorted[6];
            Assert.Equal("Enum", change.Breadcrumb);
            Assert.Equal(EnumVictim.One, change.OldValue);
            Assert.Equal(EnumVictim.Two, change.NewValue);
            
            change = sorted[7];
            Assert.Equal("Float", change.Breadcrumb);
            Assert.Equal(6.66f, change.OldValue);
            Assert.Equal(6.78f, change.NewValue);
            
            change = sorted[8];
            Assert.Equal("Int", change.Breadcrumb);
            Assert.Equal(666, change.OldValue);
            Assert.Equal(777, change.NewValue);
            
            change = sorted[9];
            Assert.Equal("InternalIsIncluded", change.Breadcrumb);
            Assert.Equal("internal", change.OldValue);
            Assert.Equal("changed internal", change.NewValue);
            
            change = sorted[10];
            Assert.Equal("PrivateIsIncluded", change.Breadcrumb);
            Assert.Equal("private", change.OldValue);
            Assert.Equal("changed", change.NewValue);
            
            change = sorted[11];
            Assert.Equal("ProtectedIsIncluded", change.Breadcrumb);
            Assert.Equal("protected", change.OldValue);
            Assert.Equal("changed protected", change.NewValue);
            
            change = sorted[12];
            Assert.Equal("SomeThing.SomeString", change.Breadcrumb);
            Assert.Equal("Some", change.OldValue);
            Assert.Equal("Changed", change.NewValue);
            
            change = sorted[13];
            Assert.Equal("String", change.Breadcrumb);
            Assert.Equal("Kewl string", change.OldValue);
            Assert.Equal("Hot string", change.NewValue);
            
            change = sorted[14];
            Assert.Equal("TimeSpan", change.Breadcrumb);
            Assert.Equal(oldState.TimeSpan, change.OldValue);
            Assert.Equal(newState.TimeSpan, change.NewValue);
        }

        [Fact]
        public void FindDifferences_works_withIgnoredField()
        {
            var oldState = new SimpleVictim
            {
                Property1 = "Old prop1",
                Property2 = "Old prop2",
                field1 = "Old field1",
                field2 = "Old field2"
            };

            var newState = new SimpleVictim
            {
                Property1 = "New prop1",
                Property2 = "New prop2",
                field1 = "New field1",
                field2 = "New field2"
            };
            
            worker.FindDifferences
            (
                oldState,
                newState,
                stateChanges,
                3,
                (field, context) => field.Name.Equals("field1"),
                (prop, context) => false
            );
            
            Assert.Equal(3, stateChanges.Changes.Count);
            
            var sorted = stateChanges.Changes.OrderBy(x => x.Breadcrumb).ToList();
            
            Assert.Equal("Old field2", sorted[0].OldValue);
            Assert.Equal("New field2", sorted[0].NewValue);

            Assert.Equal("Old prop1", sorted[1].OldValue);
            Assert.Equal("New prop1", sorted[1].NewValue);

            Assert.Equal("Old prop2", sorted[2].OldValue);
            Assert.Equal("New prop2", sorted[2].NewValue);
        }

        [Fact]
        public void FindDifferences_works_withIgnoredProperty()
        {
            var oldState = new SimpleVictim
            {
                Property1 = "Old prop1",
                Property2 = "Old prop2",
                field1 = "Old field1",
                field2 = "Old field2"
            };

            var newState = new SimpleVictim
            {
                Property1 = "New prop1",
                Property2 = "New prop2",
                field1 = "New field1",
                field2 = "New field2"
            };
            
            worker.FindDifferences
            (
                oldState,
                newState,
                stateChanges,
                3,
                (field, context) => false,
                (prop, context) => prop.Name.Equals("Property1")
            );
            
            Assert.Equal(3, stateChanges.Changes.Count);
            
            var sorted = stateChanges.Changes.OrderBy(x => x.Breadcrumb).ToList();
            
            Assert.Equal("Old field1", sorted[0].OldValue);
            Assert.Equal("New field1", sorted[0].NewValue);

            Assert.Equal("Old field2", sorted[1].OldValue);
            Assert.Equal("New field2", sorted[1].NewValue);

            Assert.Equal("Old prop2", sorted[2].OldValue);
            Assert.Equal("New prop2", sorted[2].NewValue);
        }

        [Fact]
        public void FindDifferences_works_nothingIsIgnoredWhenIgnoreFuncsAreNull()
        {
            var oldState = new SimpleVictim
            {
                Property1 = "Old prop1",
                Property2 = "Old prop2",
                field1 = "Old field1",
                field2 = "Old field2"
            };

            var newState = new SimpleVictim
            {
                Property1 = "New prop1",
                Property2 = "New prop2",
                field1 = "New field1",
                field2 = "New field2"
            };
            
            worker.FindDifferences
            (
                oldState,
                newState,
                stateChanges,
                3,
                null,
                null
            );
            
            Assert.Equal(4, stateChanges.Changes.Count);
            
            var sorted = stateChanges.Changes.OrderBy(x => x.Breadcrumb).ToList();
            
            Assert.Equal("Old field1", sorted[0].OldValue);
            Assert.Equal("New field1", sorted[0].NewValue);

            Assert.Equal("Old field2", sorted[1].OldValue);
            Assert.Equal("New field2", sorted[1].NewValue);

            Assert.Equal("Old prop1", sorted[2].OldValue);
            Assert.Equal("New prop1", sorted[2].NewValue);

            Assert.Equal("Old prop2", sorted[3].OldValue);
            Assert.Equal("New prop2", sorted[3].NewValue);
        }

        [Fact]
        public void FindDifferences_recursionDepthHandling_bailButDoNotBlow()
        {
            var oldChild = new ChildVictim {ChildString = "old child"};
            var oldState = new ParentVictim
            {
                Child = oldChild,
                ParentString = "old parent"
            };
            oldChild.Parent = oldState;
            
            var newChild = new ChildVictim {ChildString = "new child"};
            var newState = new ParentVictim
            {
                Child = newChild,
                ParentString = "new parent"
            };
            newChild.Parent = newState;

            worker.FindDifferences(oldState, newState, stateChanges, 3);
            
            Assert.Equal(4, stateChanges.Changes.Count);

            var change = stateChanges.Changes[0];
            Assert.Equal("Child.Parent.Child.ChildString", change.Breadcrumb);
            Assert.Equal("old child", change.OldValue);
            Assert.Equal("new child", change.NewValue);

            change = stateChanges.Changes[1];
            Assert.Equal("Child.Parent.ParentString", change.Breadcrumb);
            Assert.Equal("old parent", change.OldValue);
            Assert.Equal("new parent", change.NewValue);
            
            change = stateChanges.Changes[2];
            Assert.Equal("Child.ChildString", change.Breadcrumb);
            Assert.Equal("old child", change.OldValue);
            Assert.Equal("new child", change.NewValue);
            
            change = stateChanges.Changes[3];
            Assert.Equal("ParentString", change.Breadcrumb);
            Assert.Equal("old parent", change.OldValue);
            Assert.Equal("new parent", change.NewValue);
        }

        #region Victims
        public class SimpleVictim
        {
            public string Property1 { get; set; }
            public string Property2 { get; set; }
            public string field1;
            public string field2;
        }
        
        public class TheVictim
        {
            private string PrivateIsIncluded { get; set; }

            public TheVictim SetPrivate(string value)
            {
                PrivateIsIncluded = value;
                return this;
            }
            
            protected string ProtectedIsIncluded { get; set; }

            public TheVictim SetProtected(string value)
            {
                ProtectedIsIncluded = value;
                return this;
            }
            
            internal string InternalIsIncluded { get; set; }
            
            public string String { get; set; }
            public char Char { get; set; }
            public int Int { get; set; }
            public float Float { get; set; }
            public double Double { get; set; }
            public decimal Decimal { get; set; }
            public bool Bool { get; set; }
            public DateTime DateTime { get; set; }
            public TimeSpan TimeSpan { get; set; }
            public EnumVictim Enum { get; set; }
            public byte Byte { get; set; }
            public SomeThing SomeThing { get; set; }
        }
        
        public enum EnumVictim
        {
            Zero=0,
            One=1,
            Two=2
        }
        
        public class SomeThing
        {
            public string SomeString { get; set; }
        }
        
        public class ParentVictim
        {
            public ChildVictim Child { get; set; }
            public string ParentString { get; set; }
        }
        
        public class ChildVictim
        {
            public ParentVictim Parent { get; set; }
            public string ChildString { get; set; }
        }
        #endregion
    }
}