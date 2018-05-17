using System;
using System.Threading.Tasks;
using RapidCore.Locking;
using StackExchange.Redis;

namespace RapidCore.Redis.Locking
{
    /// <summary>
    /// Implementation of a <see cref="IDistributedAppLockProvider"/> that utilizes Redis as the backing store for 
    /// creating the locks
    /// </summary>
    public class RedisDistributedAppLockProvider : IDistributedAppLockProvider
    {
        private readonly IConnectionMultiplexer _redisMuxer;

        /// <summary>
        /// Create a new instance of the locker
        /// </summary>
        /// <param name="redisMuxer">A connected instance of a redis muxer</param>
        public RedisDistributedAppLockProvider(IConnectionMultiplexer redisMuxer)
        {
            _redisMuxer = redisMuxer;
        }

        /// <summary>
        /// Synchronously acquire the given lock
        /// </summary>
        /// <param name="lockName">Name of the lock to acquire</param>
        /// <param name="lockWaitTimeout">A max amount of time to wait for the lock to become available</param>
        /// <param name="lockAutoExpireTimeout">The amount of time the lock is allowed to stay in Redis before redis
        /// will auto expire the lock key</param>
        /// <returns>A <see cref="RedisDistributedAppLock"/> if the lock is grabbed, throws otherwise</returns>
        /// <exception cref="Exception"></exception>
        public IDistributedAppLock Acquire(string lockName, TimeSpan? lockWaitTimeout = default(TimeSpan?),
            TimeSpan? lockAutoExpireTimeout = default(TimeSpan?))
        {
            var handle = new RedisDistributedAppLock(_redisMuxer);
            var task = handle.AcquireLockAsync(lockName, lockWaitTimeout, lockAutoExpireTimeout);

            try
            {
                // wait for task to complete
                return task.Result;
            }
            catch (AggregateException exception)
            {
                throw exception.Flatten().InnerException;
            }
        }

        /// <summary>
        /// Asynchronously acquire the given lock
        /// </summary>
        /// <param name="lockName">Name of the lock to acquire</param>
        /// <param name="lockWaitTimeout">A max amount of time to wait for the lock to become available</param>
        /// <param name="lockAutoExpireTimeout">The amount of time the lock is allowed to stay in Redis before redis
        /// will auto expire the lock key</param>
        /// <returns>A <see cref="RedisDistributedAppLock"/> if the lock is grabbed, throws otherwise</returns>
        public async Task<IDistributedAppLock> AcquireAsync(string lockName,
            TimeSpan? lockWaitTimeout = default(TimeSpan?), TimeSpan? lockAutoExpireTimeout = default(TimeSpan?))
        {
            var handle = new RedisDistributedAppLock(_redisMuxer);
            return await handle.AcquireLockAsync(lockName, lockWaitTimeout, lockAutoExpireTimeout);
        }
    }
}