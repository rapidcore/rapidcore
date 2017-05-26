using System;

namespace RapidCore.Locking
{
    /// <summary>
    /// When implemented in a downstream locker provider, this instance contains a handle to the underlying lock instance
    /// </summary>
    public interface IDistributedAppLock : IDisposable
    {
        /// <summary>
        /// The name of the lock acquired
        /// </summary>
        string Name { get; set; }
    }
}