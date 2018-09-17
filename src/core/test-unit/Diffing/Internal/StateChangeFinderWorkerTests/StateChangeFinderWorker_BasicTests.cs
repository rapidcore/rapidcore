﻿using System;
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
                }
            }.SetPrivate("private");
            
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
                }
            }.SetPrivate("changed");

            worker.FindDifferences(oldState, newState, stateChanges, 5);

            Assert.Equal(13, stateChanges.Changes.Count);
            
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
            Assert.Equal("PrivateIsIncluded", change.Breadcrumb);
            Assert.Equal("private", change.OldValue);
            Assert.Equal("changed", change.NewValue);
            
            change = sorted[10];
            Assert.Equal("SomeThing.SomeString", change.Breadcrumb);
            Assert.Equal("Some", change.OldValue);
            Assert.Equal("Changed", change.NewValue);
            
            change = sorted[11];
            Assert.Equal("String", change.Breadcrumb);
            Assert.Equal("Kewl string", change.OldValue);
            Assert.Equal("Hot string", change.NewValue);
            
            change = sorted[12];
            Assert.Equal("TimeSpan", change.Breadcrumb);
            Assert.Equal(oldState.TimeSpan, change.OldValue);
            Assert.Equal(newState.TimeSpan, change.NewValue);
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
        public class TheVictim
        {
            private string PrivateIsIncluded { get; set; }

            public TheVictim SetPrivate(string value)
            {
                PrivateIsIncluded = value;
                return this;
            }
            
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