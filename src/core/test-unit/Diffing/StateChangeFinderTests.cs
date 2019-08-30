using System;
using System.Reflection;
using FakeItEasy;
using RapidCore.Diffing;
using RapidCore.Diffing.Internal;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Diffing
{
    public class StateChangeFinderTests
    {
        private readonly StateChangeFinder stateChangeFinder;
        private readonly StateChangeFinderWorker worker;

        private readonly Func<FieldInfo, IReadOnlyInstanceTraversalContext, bool> fieldIgnoreFn;
        private readonly Func<PropertyInfo, IReadOnlyInstanceTraversalContext, bool> propertyIgnoreFn;

        public StateChangeFinderTests()
        {
            worker = A.Fake<StateChangeFinderWorker>();
            
            stateChangeFinder = new TestableStateChangeFinder(worker);

            fieldIgnoreFn = A.Fake<Func<FieldInfo, IReadOnlyInstanceTraversalContext, bool>>();
            propertyIgnoreFn = A.Fake<Func<PropertyInfo, IReadOnlyInstanceTraversalContext, bool>>();
        }
        
        [Fact]
        public void GetChanges_ifBothStatesAreNull_noChanges()
        {
            var actual = stateChangeFinder.GetChanges(null, null, fieldIgnoreFn, propertyIgnoreFn);
            
            Assert.Empty(actual.Changes);
        }
        
        [Fact]
        public void GetChanges_throw_ifStatesAreNotSameType()
        {
            var actual = Record.Exception(() => stateChangeFinder.GetChanges("hello", 5, fieldIgnoreFn, propertyIgnoreFn));

            Assert.IsType<NotSupportedException>(actual);
            Assert.Equal("Finding changes between two different classes is not supported. Received: old=System.String, new=System.Int32", actual.Message);
        }
        
        [Fact]
        public void GetChanges_worksWith_null_new_byCalling_worker()
        {
            var newState = new BasicVictim {MyString = "yay"};

            stateChangeFinder.MaxDepth = 666;
            var actual = stateChangeFinder.GetChanges(null, newState, fieldIgnoreFn, propertyIgnoreFn);

            A.CallTo(() => worker.FindDifferences(null, newState, actual, 666, fieldIgnoreFn, propertyIgnoreFn)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void GetChanges_worksWith_old_null_byCalling_worker()
        {
            var oldState = new BasicVictim {MyString = "yay"};
            
            stateChangeFinder.MaxDepth = 666;
            var actual = stateChangeFinder.GetChanges(oldState, null, fieldIgnoreFn, propertyIgnoreFn);

            A.CallTo(() => worker.FindDifferences(oldState, null, actual, 666, fieldIgnoreFn, propertyIgnoreFn)).MustHaveHappenedOnceExactly();
        }
        
        [Fact]
        public void GetChanges_SameInstance_noChanges()
        {
            var oldState = new BasicVictim {MyString = "yay"};

            var actual = stateChangeFinder.GetChanges(oldState, oldState, fieldIgnoreFn, propertyIgnoreFn);
            
            Assert.Empty(actual.Changes);
        }

        #region Victims
        public class BasicVictim
        {
            public string MyString { get; set; }
        }
        
        public class TestableStateChangeFinder : StateChangeFinder
        {
            public TestableStateChangeFinder(StateChangeFinderWorker worker) : base(worker)
            {
            }
        }
        #endregion
    }
}