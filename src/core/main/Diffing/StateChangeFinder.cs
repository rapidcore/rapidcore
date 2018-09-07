using System;
using RapidCore.Diffing.Internal;
using RapidCore.Reflection;

namespace RapidCore.Diffing
{
    public class StateChangeFinder
    {
        private readonly StateChangeFinderWorker worker;

        public StateChangeFinder()
        {
            worker = new StateChangeFinderWorker(new InstanceAnalyzer());
        }
        
        /// <summary>
        /// Only meant to be used by unit tests
        /// </summary>
        protected StateChangeFinder(StateChangeFinderWorker worker)
        {
            this.worker = worker;
        }
        
        /// <summary>
        /// The maximum depth allowed when following
        /// references
        /// </summary>
        public virtual int MaxDepth { get; set; } = 10;
        
        public virtual StateChanges GetChanges(object oldState, object newState)
        {
            var changes = new StateChanges
            {
                OldState = oldState,
                NewState = newState
            };
            
            //
            // both null?
            //
            if (oldState == null && newState == null)
            {
                // no changes, obviously
                return changes;
            }
            
            //
            // same instance?
            //
            if (oldState != null && oldState.Equals(newState))
            {
                // no changes, obviously
                return changes;
            }

            //
            // different types?
            //
            if (oldState != null && newState != null && oldState.GetType() != newState.GetType())
            {
                throw new NotSupportedException($"Finding changes between two different classes is not supported. Received: old={oldState.GetType().FullName}, new={newState.GetType().FullName}");
            }
            
            //
            // ok, time to work
            //
            worker.FindDifferences(oldState, newState, changes, MaxDepth);

            return changes;
        }
    }
}