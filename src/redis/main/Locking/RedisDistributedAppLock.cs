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
        private readonly IConnectionMultiplexer _redisMuxer;
        private IDatabase _redisDb;
        private readonly Random _rng;

        /// <summary>
        /// Create a new instance of an actual distributed app lock
        /// </summary>
        /// <param name="redisMuxer">An instance of a connected Redis Muxer</param>
        /// <param name="rng">A pre-instantiated random generator to spread poll-waits</param>
        public RedisDistributedAppLock(IConnectionMultiplexer redisMuxer, Random rng)
        {
            _redisMuxer = redisMuxer;
            _rng = rng;
        }

        /// <summary>
        /// The name of the lock acquired
        /// </summary>
        public string Name { get; private set; }

        public bool IsActive { get; private set; }

        /// <summary>
        /// A unique lock handle for this instance of the lock
        /// </summary>
        public string LockHandle { get; protected set; }

        /// <summary>
        /// Acquire the lock with the underlying Redis instance
        /// </summary>
        /// <param name="lockName">Name of the lock to acquire</param>
        /// <param name="lockWaitTimeout">When set, the amount of time to wait for the lock to become available</param>
        /// <param name="lockAutoExpireTimeout">The amount of time the lock is allowed to stay in Redis before redis will auto expire the lock key</param>
        /// <returns><c>this</c> upon successful lock grab (for a fluent interface)</returns>
        /// <exception cref="DistributedAppLockException"></exception>
        public async Task<IDistributedAppLock> AcquireLockAsync(string lockName, TimeSpan? lockWaitTimeout = null,
            TimeSpan? lockAutoExpireTimeout = null)
        {
            var stopWatch = new Stopwatch();
            try
            {
                LockHandle = Guid.NewGuid().ToString("N");
                var timeoutProvided = lockWaitTimeout.HasValue;
                if (!timeoutProvided)
                {
                    lockWaitTimeout = TimeSpan.Zero;
                }

                if (lockAutoExpireTimeout == null)
                {
                    lockAutoExpireTimeout = TimeSpan.FromDays(1);
                }

                _redisDb = _redisMuxer.GetDatabase();
                stopWatch.Start();
                do
                {
                    var lockWasAcquired = _redisDb.LockTake(lockName, LockHandle, lockAutoExpireTimeout.Value);

                    if (!lockWasAcquired && !timeoutProvided)
                    {
                        throw new DistributedAppLockException("Unable to acquire lock")
                        {
                            Reason = DistributedAppLockExceptionReason.LockAlreadyAcquired,
                        };
                    }

                    if (!lockWasAcquired)
                    {
                        var timeout = _rng.Next(
                            1,
                            (int) Math.Min(2500, lockWaitTimeout.Value.TotalMilliseconds)
                        ); // wait between 1 ms and either  2,5 seconds OR lockwaitTimeout if this is smaller than 2.5 secondss
                        await Task.Delay(TimeSpan.FromMilliseconds(timeout));
                        continue;
                    }

                    // Lock was acquired, we're happy
                    IsActive = true;
                    Name = lockName;
                    break;
                } while (stopWatch.Elapsed.TotalSeconds < lockWaitTimeout.Value.TotalSeconds);

                if (!IsActive)
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
                    Reason = lockWaitTimeout == null
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
        /// Determines whether the current lock instance is <see cref="IsActive"/> and has a name that matches the given
        /// parameter
        /// </summary>
        /// <param name="name">The name of the lock to asssert that is currently taken</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ThrowIfNotActiveWithGivenName(string name)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException(
                    $"Lock precondition mismatch, required IsActive=true with name '{name}' but IsActive=false with name '{this.Name}'");
            }

            if (!Name.Equals(name))
            {
                throw new InvalidOperationException(
                    $"Lock precondition mismatch, required IsActive=true with name '{name}' but IsActive=true with name '{this.Name}'");
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
                _redisDb?.LockRelease(Name, LockHandle);
                Name = default(string);
                LockHandle = default(string);
                IsActive = false;
            }

            _disposedValue = true;
        }
    }
}