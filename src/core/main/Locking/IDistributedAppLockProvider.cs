using System;
using System.Threading.Tasks;

namespace RapidCore.Locking
{
    /// <summary>
    /// Provides an abstraction for acquiring a <see cref="IDistributedAppLock"/> instance that ensures locking across
    /// all application pools / dotnet core processes running that tries to acquire a lock of the same name
    /// </summary>
    public interface IDistributedAppLockProvider
    {
        /// <summary>
        /// When implemented in a downstream lock class, it will try to acquire the lock in a blocking (sync) manner
        ///
        /// If lockWaitTimeout is defined it will wait / retry for the given amount of time before throwing an exception
        /// and giving up on acquiring the lock
        /// 
        /// Providing a <paramref name="lockAutoExpireTimeout"/> will enable the distributed app lock provider to automatically
        /// release stale locks in case the application has crashed and didn't release the lock
        /// </summary>
        /// <exception cref="DistributedAppLockException">Thrown if the lock cannot be acquired</exception>
        /// <param name="lockName">The name of the lock to acquire</param>
        /// <param name="lockWaitTimeout">The time to wait / retry acquiring the lock</param>
        /// <param name="lockAutoExpireTimeout">The amount of time before the lock should automatically be expired</param>
        IDistributedAppLock Acquire(string lockName, TimeSpan? lockWaitTimeout = null, TimeSpan? lockAutoExpireTimeout = null);

        ///  <summary>
        ///  When implemented in a downstream lock class, it will try to acquire the lock in a non-blocking (async) manner
        ///  If lockWaitTimeout is defined it will wait / retry for the given amount of time before throwing an exception
        ///  and giving up on acquiring the lock
        /// Providing a <paramref name="lockAutoExpireTimeout"/> will enable the distributed app lock provider to automatically
        /// release stale locks in case the application has crashed and didn't release the lock
        ///  </summary>
        ///  <exception cref="DistributedAppLockException">Thrown if the lock cannot be acquired</exception>
        ///  <param name="lockName">The name of the lock to acquire</param>
        ///  <param name="lockWaitTimeout">The time to wait / retry acquiring the lock</param>
        /// <param name="lockAutoExpireTimeout">The amount of time before the lock should automatically be expired</param>
        /// <returns></returns>
        Task<IDistributedAppLock> AcquireAsync(string lockName, TimeSpan? lockWaitTimeout = null, TimeSpan? lockAutoExpireTimeout = null);
    }
}