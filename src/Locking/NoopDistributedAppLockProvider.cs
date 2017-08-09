using System;
using System.Threading.Tasks;
using RapidCore.Threading;

namespace RapidCore.Locking
{
    /// <summary>
    /// Provides an easy-to-get-started distributed app locker. Note though
    /// that IT DOES NOT LOCK ANYTHING.
    /// 
    /// It is meant to help you get started on coding while you figure out / wait for
    /// servers to be available (in this case, maybe check out https://docker.com).
    /// </summary>
    [Obsolete("This should not be used in real systems", false)]
    public class NoopDistributedAppLockProvider : IDistributedAppLockProvider
    {
        public virtual IDistributedAppLock Acquire(string lockName, TimeSpan? lockWaitTimeout = null)
        {
            return AcquireAsync(lockName, lockWaitTimeout).AwaitSync();
        }

        public virtual async Task<IDistributedAppLock> AcquireAsync(string lockName, TimeSpan? lockWaitTimeout = null)
        {
            return await Task.FromResult(new NoopDistributedAppLock
            {
                IsActive = true,
                Name = lockName
            });
        }
    }
}