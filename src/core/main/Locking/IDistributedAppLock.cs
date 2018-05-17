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
        string Name { get; }

        /// <summary>
        /// Determines whether the lock has been taken in the underlying source and is still active
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// When implemented in a downstream provider it will verify that the current instance of the lock is in an
        /// active (locked) state and has the name given to the method
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="InvalidOperationException">
        /// When the lock is either not active, or has a different name than provided in <paramref name="name"/>
        /// </exception>
        void ThrowIfNotActiveWithGivenName(string name);
    }
}