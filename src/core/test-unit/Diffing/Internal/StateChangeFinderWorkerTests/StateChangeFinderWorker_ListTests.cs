using System.Collections;
using System.Collections.Generic;
using RapidCore.Diffing;
using RapidCore.Diffing.Internal;
using RapidCore.Reflection;
using Xunit;

namespace RapidCore.UnitTests.Diffing.Internal.StateChangeFinderWorkerTests
{
    public class StateChangeFinderWorker_ListTests
    {
        private readonly StateChangeFinderWorker worker;
        private readonly StateChanges stateChanges;

        public StateChangeFinderWorker_ListTests()
        {
            stateChanges = new StateChanges();

            worker = new StateChangeFinderWorker(new InstanceAnalyzer());
        }
        
        [Fact]
        public void GetChanges_ilist()
        {
            var oldState = new IListVictim
            {
                IList = new List<int> { 11, 22 }
            };
            
            var newState = new IListVictim
            {
                IList = new List<int> { 33, 44 }
            };

            worker.FindDifferences(oldState, newState, stateChanges, 5);
            
            Assert.Equal(2, stateChanges.Changes.Count);
        }
        
        [Fact]
        public void GetChanges_list()
        {
            var oldState = new ListVictim
            {
                List = new List<int> { 11, 22 }
            };
            
            var newState = new ListVictim
            {
                List = new List<int> { 33, 44 }
            };

            worker.FindDifferences(oldState, newState, stateChanges, 5);
            
            Assert.Equal(2, stateChanges.Changes.Count);
        }
        
        [Fact]
        public void GetChanges_customlist()
        {
            var oldState = new CustomListVictim
            {
                CustomList = new CustomList { 11, 22 }
            };
            
            var newState = new CustomListVictim
            {
                CustomList = new CustomList { 33, 44 }
            };

            worker.FindDifferences(oldState, newState, stateChanges, 5);
            
            Assert.Equal(2, stateChanges.Changes.Count);
        }
        
        [Fact]
        public void GetChanges_ienumerable_generic()
        {
            var oldState = new IEnumerableGenericVictim
            {
                IEnumerableGeneric = new List<int> { 11, 22 }
            };
            
            var newState = new IEnumerableGenericVictim
            {
                IEnumerableGeneric = new List<int> { 33, 44 }
            };

            worker.FindDifferences(oldState, newState, stateChanges, 5);
            
            Assert.Equal(2, stateChanges.Changes.Count);
        }
        
        [Fact]
        public void GetChanges_ienumerable()
        {
            var oldState = new IEnumerableVictim
            {
                IEnumerable = new List<int> { 11, 22 }
            };
            
            var newState = new IEnumerableVictim
            {
                IEnumerable = new List<int> { 33, 44 }
            };

            worker.FindDifferences(oldState, newState, stateChanges, 5);
            
            Assert.Equal(2, stateChanges.Changes.Count);
        }
        
        [Fact]
        public void GetChanges_dictionary()
        {
            var oldState = new DictionaryVictim
            {
                Dictionary = new Dictionary<string, int>
                {
                    { "one", 11 },
                    { "two", 22 }
                }
            };
            
            var newState = new DictionaryVictim
            {
                Dictionary = new Dictionary<string, int>
                {
                    { "one", 33 },
                    { "five", 55 }
                }
            };

            worker.FindDifferences(oldState, newState, stateChanges, 5);
            
            Assert.Equal(3, stateChanges.Changes.Count);

            var change = stateChanges.Changes[0];
            Assert.Equal("Dictionary[one]", change.Breadcrumb);
            Assert.Equal(11, change.OldValue);
            Assert.Equal(33, change.NewValue);
            
            change = stateChanges.Changes[1];
            Assert.Equal("Dictionary[two]", change.Breadcrumb);
            Assert.Equal(22, change.OldValue);
            Assert.Equal(null, change.NewValue);
            
            change = stateChanges.Changes[2];
            Assert.Equal("Dictionary[five]", change.Breadcrumb);
            Assert.Equal(null, change.OldValue);
            Assert.Equal(55, change.NewValue);
        }
      

        #region Victims
        public class IListVictim
        {
            public IList<int> IList { get; set; }
        }
        
        public class ListVictim
        {
            public List<int> List { get; set; }
        }
        
        public class CustomListVictim
        {
            public CustomList CustomList { get; set; }
        }
        
        public class CustomList : List<int>
        {
        }
        
        public class IEnumerableGenericVictim
        {
            public IEnumerable<int> IEnumerableGeneric { get; set; }
        }
        
        public class IEnumerableVictim
        {
            public IEnumerable IEnumerable { get; set; }
        }
        
        public class DictionaryVictim
        {
            public Dictionary<string, int> Dictionary { get; set; }
        }
        #endregion
    }
}