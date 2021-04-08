using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace RapidCore.Locking
{
    public class InMemoryDistributedAppLockProvider : IDistributedAppLockProvider
    {
        private readonly Random _rng;
        private readonly ConcurrentDictionary<string, InMemoryDistributedAppLockSemaphore> _semaphores;

        public InMemoryDistributedAppLockProvider()
        {
            _rng = new Random();
            _semaphores = new ConcurrentDictionary<string, InMemoryDistributedAppLockSemaphore>();
        }
        
        public IDistributedAppLock Acquire(string lockName, TimeSpan? lockWaitTimeout = null, TimeSpan? lockAutoExpireTimeout = null)
        {
            var handle = new InMemoryDistributedAppLock(_rng, _semaphores);
            var task = handle.AcquireLockAsync(lockName, lockWaitTimeout);

            try
            {
                // wait for task to complete
                return task.GetAwaiter().GetResult();
            }
            catch (AggregateException exception)
            {
                var innerException = exception.Flatten().InnerException;
                if (innerException != null) throw innerException;
                throw;
            }
        }

        public async Task<IDistributedAppLock> AcquireAsync(string lockName, TimeSpan? lockWaitTimeout = null, TimeSpan? lockAutoExpireTimeout = null)
        {
            var handle = new InMemoryDistributedAppLock(_rng, _semaphores);
            return await handle.AcquireLockAsync(lockName, lockWaitTimeout);
        }
    }
}