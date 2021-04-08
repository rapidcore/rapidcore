using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RapidCore.Locking
{
    public class InMemoryDistributedAppLock : IDistributedAppLock
    {
        private bool _disposedValue; // To detect redundant calls
        private readonly Random _rng;
        private readonly ConcurrentDictionary<string, InMemoryDistributedAppLockSemaphore> _semaphores;

        public InMemoryDistributedAppLock(Random rng, ConcurrentDictionary<string, InMemoryDistributedAppLockSemaphore> semaphores)
        {
            _rng = rng;
            _semaphores = semaphores;
        }

        /// <summary>
        /// Acquire the lock
        /// </summary>
        /// <param name="lockName">Name of the lock to acquire</param>
        /// <param name="lockWaitTimeout">When set, the amount of time to wait for the lock to become available</param>
        public async Task<IDistributedAppLock> AcquireLockAsync(string lockName, TimeSpan? lockWaitTimeout = null)
        {
            WasAcquiredInstantly = true;
            var timeoutProvided = lockWaitTimeout.HasValue;
            if (!timeoutProvided)
            {
                lockWaitTimeout = TimeSpan.Zero;
            }
            
            InMemoryDistributedAppLockSemaphore semaphore;
            lock (string.Intern(lockName))
            {
                semaphore = _semaphores.GetOrAdd(lockName, _ => new InMemoryDistributedAppLockSemaphore());
                semaphore.IncrementReferenceCount(); // Remember that this lock is being referenced
            }
            
            var timeout = TimeSpan.Zero;
            
            var stopWatch = new Stopwatch();
            do
            {
                var lockWasAcquired = await semaphore.WaitAsync(timeout);
                
                if (!lockWasAcquired && !timeoutProvided)
                {
                    LockRelease(lockName);
                    throw new DistributedAppLockException("Unable to acquire lock")
                    {
                        Reason = DistributedAppLockExceptionReason.LockAlreadyAcquired,
                    };
                }

                if (!lockWasAcquired)
                {
                    WasAcquiredInstantly = false;
                    timeout = TimeSpan.FromMilliseconds(_rng.Next(
                        1,
                        (int) Math.Min(2500, lockWaitTimeout.Value.TotalMilliseconds)
                    )); // wait between 1 ms and either 2.5 seconds OR lockWaitTimeout if this is smaller than 2.5 seconds
                    continue;
                }

                IsActive = true;
                Name = lockName;
                break;
            } while (stopWatch.Elapsed.TotalSeconds < lockWaitTimeout.Value.TotalSeconds);

            if (!IsActive)
            {
                LockRelease(lockName);
                throw new DistributedAppLockException("Timeout while acquiring lock")
                {
                    Reason = DistributedAppLockExceptionReason.Timeout,
                };
            }
            
            TimeUsedToAcquire = stopWatch.Elapsed;
            return this;
        } 

        /// <summary>
        /// Determines whether the current lock instance is active (<see cref="IsActive"/>) and has a name that matches
        /// the given parameter
        /// </summary>
        /// <param name="name">The name of the lock to assert that is currently taken</param>
        /// <exception cref="InvalidOperationException">If the lock is not active or the name of the lock does not match
        /// the provided name</exception>
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
        /// Release the lock (if it is active) and stop referencing the semaphore
        /// </summary>
        private void LockRelease(string lockName)
        {
            lock (string.Intern(lockName))
            {
                var semaphore = _semaphores[lockName];
                if (IsActive)
                {
                    // Let someone else take the lock
                    semaphore.Release();
                }
                semaphore.DecrementReferenceCount();
                if (semaphore.ReferenceCount == 0) // If no one is referencing this semaphore, remove it to save space
                {
                    _semaphores.TryRemove(lockName, out _);
                }
            }
        }

        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public bool WasAcquiredInstantly { get; private set; }
        public TimeSpan TimeUsedToAcquire { get; private set; }
        
        /// <summary>
        /// Dispose of the lock instance and release the lock
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) below.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                LockRelease(Name);
                Name = default;
                IsActive = false;
            }

            _disposedValue = true;
        }

        ~InMemoryDistributedAppLock()
        {
            Dispose(false);
        }
    }
}