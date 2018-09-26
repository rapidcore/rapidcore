using System.Collections.Generic;

namespace RapidCore.Diffing
{
    /// <summary>
    /// A set of changes in state of an object
    /// </summary>
    public class StateChanges
    {
        /// <summary>
        /// The original/old state
        /// </summary>
        public virtual object OldState { get; set; }
        
        /// <summary>
        /// The new state
        /// </summary>
        public virtual object NewState { get; set; }
        
        /// <summary>
        /// The actual changes in state
        /// </summary>
        public virtual List<StateChange> Changes { get; set; } = new List<StateChange>();
    }
}