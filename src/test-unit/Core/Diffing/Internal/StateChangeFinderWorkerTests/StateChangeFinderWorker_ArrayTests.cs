using RapidCore.Diffing;
using RapidCore.Diffing.Internal;
using RapidCore.Reflection;
using Xunit;

namespace UnitTests.Core.Diffing.Internal.StateChangeFinderWorkerTests
{
    public class StateChangeFinderWorker_ArrayTests
    {
        private readonly StateChangeFinderWorker worker;
        private readonly StateChanges stateChanges;

        public StateChangeFinderWorker_ArrayTests()
        {
            stateChanges = new StateChanges();

            worker = new StateChangeFinderWorker(new InstanceTraverser());
        }
        
        [Fact]
        public void FindDifferences_Array_int_bothEmpty()
        {
            var oldState = new ArrayVictim
            {
                IntArray = new int[5]
            };
            
            var newState = new ArrayVictim
            {
                IntArray = new int[5]
            };

            worker.FindDifferences(oldState, newState, stateChanges, 5);
            
            Assert.Empty(stateChanges.Changes);
        }
        
        [Fact]
        public void FindDifferences_Array_int_sameLength_differentValues()
        {
            var oldState = new ArrayVictim
            {
                IntArray = new [] { 11, 22 ,33 }
            };
            
            var newState = new ArrayVictim
            {
                IntArray = new [] { 44, 55, 66 }
            };

            worker.FindDifferences(oldState, newState, stateChanges, 5);
            
            Assert.Equal(3, stateChanges.Changes.Count);
            
            var change = stateChanges.Changes[0];
            Assert.Equal("IntArray[0]", change.Breadcrumb);
            Assert.Equal(11, change.OldValue);
            Assert.Equal(44, change.NewValue);
            
            change = stateChanges.Changes[1];
            Assert.Equal("IntArray[1]", change.Breadcrumb);
            Assert.Equal(22, change.OldValue);
            Assert.Equal(55, change.NewValue);
            
            change = stateChanges.Changes[2];
            Assert.Equal("IntArray[2]", change.Breadcrumb);
            Assert.Equal(33, change.OldValue);
            Assert.Equal(66, change.NewValue);
        }
        
        [Fact]
        public void FindDifferences_Array_int_newIsLonger_differentValues()
        {
            var oldState = new ArrayVictim
            {
                IntArray = new [] { 11, 22 ,33 }
            };
            
            var newState = new ArrayVictim
            {
                IntArray = new [] { 44, 55, 66, 77 }
            };

            worker.FindDifferences(oldState, newState, stateChanges, 5);
            
            Assert.Equal(4, stateChanges.Changes.Count);
            
            var change = stateChanges.Changes[0];
            Assert.Equal("IntArray[0]", change.Breadcrumb);
            Assert.Equal(11, change.OldValue);
            Assert.Equal(44, change.NewValue);
            
            change = stateChanges.Changes[1];
            Assert.Equal("IntArray[1]", change.Breadcrumb);
            Assert.Equal(22, change.OldValue);
            Assert.Equal(55, change.NewValue);
            
            change = stateChanges.Changes[2];
            Assert.Equal("IntArray[2]", change.Breadcrumb);
            Assert.Equal(33, change.OldValue);
            Assert.Equal(66, change.NewValue);
            
            change = stateChanges.Changes[3];
            Assert.Equal("IntArray[3]", change.Breadcrumb);
            Assert.Equal(null, change.OldValue);
            Assert.Equal(77, change.NewValue);
        }
        
        [Fact]
        public void FindDifferences_Array_int_newIsLonger_differentValues_withIgnore()
        {
            var oldState = new ArrayVictim
            {
                IntArray = new [] { 11, 22, 33 }
            };
            
            var newState = new ArrayVictim
            {
                IntArray = new [] { 44, 55, 66, 77 }
            };

            worker.FindDifferences
            (
                oldState,
                newState,
                stateChanges,
                5,
                (field, context) => false,
                (prop, context) => context.BreadcrumbAsString.Equals("IntArray[3]")
            );
            
            Assert.Equal(3, stateChanges.Changes.Count);
            
            var change = stateChanges.Changes[0];
            Assert.Equal("IntArray[0]", change.Breadcrumb);
            Assert.Equal(11, change.OldValue);
            Assert.Equal(44, change.NewValue);
            
            change = stateChanges.Changes[1];
            Assert.Equal("IntArray[1]", change.Breadcrumb);
            Assert.Equal(22, change.OldValue);
            Assert.Equal(55, change.NewValue);
            
            change = stateChanges.Changes[2];
            Assert.Equal("IntArray[2]", change.Breadcrumb);
            Assert.Equal(33, change.OldValue);
            Assert.Equal(66, change.NewValue);
        }
        
        [Fact]
        public void FindDifferences_Array_int_oldIsLonger_differentValues()
        {
            var oldState = new ArrayVictim
            {
                IntArray = new [] { 11, 22 ,33 }
            };
            
            var newState = new ArrayVictim
            {
                IntArray = new [] { 44, 55 }
            };

            worker.FindDifferences(oldState, newState, stateChanges, 5);
            
            Assert.Equal(3, stateChanges.Changes.Count);
            
            var change = stateChanges.Changes[0];
            Assert.Equal("IntArray[0]", change.Breadcrumb);
            Assert.Equal(11, change.OldValue);
            Assert.Equal(44, change.NewValue);
            
            change = stateChanges.Changes[1];
            Assert.Equal("IntArray[1]", change.Breadcrumb);
            Assert.Equal(22, change.OldValue);
            Assert.Equal(55, change.NewValue);
            
            change = stateChanges.Changes[2];
            Assert.Equal("IntArray[2]", change.Breadcrumb);
            Assert.Equal(33, change.OldValue);
            Assert.Equal(null, change.NewValue);
        }
        
        [Fact]
        public void FindDifferences_Array_int_oldIsLonger_differentValues_withIgnore()
        {
            var oldState = new ArrayVictim
            {
                IntArray = new [] { 11, 22, 33 }
            };
            
            var newState = new ArrayVictim
            {
                IntArray = new [] { 44, 55 }
            };

            worker.FindDifferences
            (
                oldState,
                newState,
                stateChanges,
                5,
                (field, context) => false,
                (prop, context) => context.BreadcrumbAsString.Equals("IntArray[2]")
            );
            
            Assert.Equal(2, stateChanges.Changes.Count);
            
            var change = stateChanges.Changes[0];
            Assert.Equal("IntArray[0]", change.Breadcrumb);
            Assert.Equal(11, change.OldValue);
            Assert.Equal(44, change.NewValue);
            
            change = stateChanges.Changes[1];
            Assert.Equal("IntArray[1]", change.Breadcrumb);
            Assert.Equal(22, change.OldValue);
            Assert.Equal(55, change.NewValue);
        }
        
        [Fact]
        public void FindDifferences_Array_int_sameValues_differentOrder()
        {
            var oldState = new ArrayVictim
            {
                IntArray = new [] { 11, 22 ,33 }
            };
            
            var newState = new ArrayVictim
            {
                IntArray = new [] { 22, 33, 11 }
            };

            worker.FindDifferences(oldState, newState, stateChanges, 5);
            
            Assert.Equal(3, stateChanges.Changes.Count);
            
            var change = stateChanges.Changes[0];
            Assert.Equal("IntArray[0]", change.Breadcrumb);
            Assert.Equal(11, change.OldValue);
            Assert.Equal(22, change.NewValue);
            
            change = stateChanges.Changes[1];
            Assert.Equal("IntArray[1]", change.Breadcrumb);
            Assert.Equal(22, change.OldValue);
            Assert.Equal(33, change.NewValue);
            
            change = stateChanges.Changes[2];
            Assert.Equal("IntArray[2]", change.Breadcrumb);
            Assert.Equal(33, change.OldValue);
            Assert.Equal(11, change.NewValue);
        }
        
        [Fact]
        public void FindDifferences_Array_int_sameValues_differentOrder_withIgnore()
        {
            var oldState = new ArrayVictim
            {
                IntArray = new [] { 11, 22 ,33 }
            };
            
            var newState = new ArrayVictim
            {
                IntArray = new [] { 22, 33, 11 }
            };

            worker.FindDifferences
            (
                oldState,
                newState,
                stateChanges,
                5,
                (field, context) => false,
                (prop, context) => context.BreadcrumbAsString.Equals("IntArray[1]")
            );
            
            Assert.Equal(2, stateChanges.Changes.Count);
            
            var change = stateChanges.Changes[0];
            Assert.Equal("IntArray[0]", change.Breadcrumb);
            Assert.Equal(11, change.OldValue);
            Assert.Equal(22, change.NewValue);
            
            change = stateChanges.Changes[1];
            Assert.Equal("IntArray[2]", change.Breadcrumb);
            Assert.Equal(33, change.OldValue);
            Assert.Equal(11, change.NewValue);
        }

        [Fact]
        public void FindDifferences_Array_complex()
        {
            var oldState = new ComplexArrayVictim
            {
                ComplexArray = new[] {new Complex(), new Complex()}
            };
            
            var newState = new ComplexArrayVictim
            {
                ComplexArray = new[] {new Complex(), new Complex { String = "different" }}
            };
            
            worker.FindDifferences(oldState, newState, stateChanges, 5);
            
            Assert.Equal(1, stateChanges.Changes.Count);
            
            var change = stateChanges.Changes[0];
            Assert.Equal("ComplexArray[1].String", change.Breadcrumb);
            Assert.Equal("yo", change.OldValue);
            Assert.Equal("different", change.NewValue);
        }

        [Fact]
        public void FindDifferences_Array_complex_withIgnore()
        {
            var oldState = new ComplexArrayVictim
            {
                ComplexArray = new[] {new Complex(), new Complex(), new Complex() }
            };
            
            var newState = new ComplexArrayVictim
            {
                ComplexArray = new[] {new Complex(), new Complex { String = "different" }, new Complex { String = "also different" } }
            };

            worker.FindDifferences
            (
                oldState,
                newState,
                stateChanges,
                5,
                (field, context) => false,
                (prop, context) => context.BreadcrumbAsString.Equals("ComplexArray[2]")
            );
            
            Assert.Equal(1, stateChanges.Changes.Count);
            
            var change = stateChanges.Changes[0];
            Assert.Equal("ComplexArray[1].String", change.Breadcrumb);
            Assert.Equal("yo", change.OldValue);
            Assert.Equal("different", change.NewValue);
        }
        

        #region Victims
        public class ArrayVictim
        {
            public int[] IntArray { get; set; }
        }
        
        public class ComplexArrayVictim
        {
            public Complex[] ComplexArray { get; set; }
        }
        
        public class Complex
        {
            public string String { get; set; } = "yo";
        }
        #endregion
    }
}