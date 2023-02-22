using System;
using System.Reflection;
using RapidCore.Diffing.Internal;
using RapidCore.Reflection;

namespace RapidCore.Diffing
{
    public class StateChangeFinder
    {
        private readonly Func<StateChangeFinderWorker> workerGenerator;

        public StateChangeFinder()
        {
            this.workerGenerator = () => new StateChangeFinderWorker(new InstanceTraverser());
        }
        
        /// <summary>
        /// Only meant to be used by unit tests
        /// </summary>
        protected StateChangeFinder(Func<StateChangeFinderWorker> workerGenerator)
        {
            this.workerGenerator = workerGenerator;
        }
        
        /// <summary>
        /// The maximum depth allowed when following
        /// references
        /// </summary>
        public virtual int MaxDepth { get; set; } = 10;
        
        public virtual StateChanges GetChanges
        (
            object oldState,
            object newState,
            Func<FieldInfo, IReadOnlyInstanceTraversalContext, bool> fieldIgnoreFunc = null,
            Func<PropertyInfo, IReadOnlyInstanceTraversalContext, bool> propertyIgnoreFunc = null
        )
        {
            //
            // Create a new worker, as a worker contains state
            //
            var worker = workerGenerator();
            
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
            worker.FindDifferences(oldState, newState, changes, MaxDepth, fieldIgnoreFunc, propertyIgnoreFunc);

            return changes;
        }
    }
}