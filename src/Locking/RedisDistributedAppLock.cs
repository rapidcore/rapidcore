using System;
using System.Diagnostics;
using System.Threading.Tasks;
using RapidCore.Locking;
using StackExchange.Redis;

namespace RapidCore.Redis.Locking
{
    public class RedisDistributedAppLock : IDistributedAppLock
    {
        private bool _disposedValue = false; // To detect redundant calls
        private IConnectionMultiplexer _redisMuxer;
        private IDatabase _redisDb;

        /// <summary>
        /// Create a new instance of an actual distributed app lock
        /// </summary>
        /// <param name="redisMuxer">An instance of a connected Redis Muxer</param>
        public RedisDistributedAppLock(IConnectionMultiplexer redisMuxer)
        {
            _redisMuxer = redisMuxer;
        }

        /// <summary>
        /// The name of the lock acquired
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Determines whether the lock has actually been acquired with the underlying Redis database
        /// </summary>
        public bool HasAcquiredLock { get; protected set; }

        /// <summary>
        /// A unique lock handle for this instance of the lock
        /// </summary>
        public string LockHandle { get; protected set; }
        
        /// <summary>
        /// Acquire the lock with the underlying Redis instance
        /// </summary>
        /// <param name="lockName">Name of the lock to acquire</param>
        /// <param name="timeout">When set, the amount of time to wait for the lock to become available</param>
        /// <returns><c>this</c> upon successful lock grab (for a fluent interface)</returns>
        /// <exception cref="DistributedAppLockException"></exception>
        public async Task<IDistributedAppLock> AcquireLockAsync(string lockName, TimeSpan? timeout = null)
        {
            var stopWatch = new Stopwatch();
            try
            {
                LockHandle = Guid.NewGuid().ToString("N");
                var noTimeoutProvided = timeout == null;
                if (noTimeoutProvided)
                {
                    timeout = TimeSpan.Zero;
                }

                _redisDb = _redisMuxer.GetDatabase();
                stopWatch.Start();
                do
                {
                    var lockWasAcquired = _redisDb.LockTake(lockName, LockHandle, TimeSpan.FromDays(1));

                    if (!lockWasAcquired && noTimeoutProvided)
                    {
                        throw new DistributedAppLockException("Unable to acquire lock")
                        {
                            Reason = DistributedAppLockExceptionReason.LockAlreadyAcquired,
                        };
                    }

                    if (!lockWasAcquired)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(50));
                        continue;
                    }

                    // Lock was acquired, we're happy
                    HasAcquiredLock = true;
                    Name = lockName;
                    break;
                } 
                while (stopWatch.Elapsed.TotalSeconds < timeout.Value.TotalSeconds);

                if (!HasAcquiredLock)
                {
                    throw new DistributedAppLockException("Timeout while acquiring lock")
                    {
                        Reason = DistributedAppLockExceptionReason.Timeout,
                    };
                }

                return this;
            }
            catch (TimeoutException tex)
            {
                var ex = new DistributedAppLockException($"Unable to acquire lock: '{lockName}'", tex)
                {
                    Reason = timeout == null
                        ? DistributedAppLockExceptionReason.LockAlreadyAcquired
                        : DistributedAppLockExceptionReason.Timeout,
                };
                throw ex;
            }
            catch (DistributedAppLockException)
            {
                throw; // simply rethrow these types of exceptions to avoid being trapped in the generic Exception catcher
            }
            catch (Exception rex)
            {
                var ex = new DistributedAppLockException($"Unable to acquire lock: '{lockName}'", rex)
                {
                    Reason = DistributedAppLockExceptionReason.SeeInnerException,
                };
                throw ex;
            }
            finally
            {
                stopWatch?.Stop();
                stopWatch = null;
            }
        }

        /// <summary>
        /// Dispose of the lock instance and release the lock with the underlying Redis system
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                // DISPOSE THE UNDERLYING REDIS STUFF
                // TODO Lock release can return false!!! So deal with that biatch..
                _redisDb.LockRelease(Name, LockHandle);
                Name = default(string);
                LockHandle = default(string);
            }

            _disposedValue = true;
        }
    }
}