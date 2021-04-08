using System;
using System.Threading.Tasks;
using RapidCore.Locking;
using Xunit;

namespace UnitTests.Core.Locking
{
    public class InMemoryDistributedAppLockProviderTest
    {
        [Fact]
        public void Test_using_multiple_times_work()
        {
            var lockName = "first-lock";
            
            var locker = new InMemoryDistributedAppLockProvider();
            using (locker.Acquire(lockName))
            {
                // mutual exclusion scope here
            }
            using (locker.Acquire(lockName))
            {
                // mutual exclusion scope here
            }
            using (locker.Acquire(lockName))
            {
                // mutual exclusion scope here
            }
        }

        [Fact]
        public void Test_cannot_acquire_lock_twice()
        {
            var lockName = "second-lock";
            
            var locker = new InMemoryDistributedAppLockProvider();

            using (locker.Acquire(lockName, TimeSpan.FromSeconds(1)))
            {
                var ex = Assert.Throws<DistributedAppLockException>(() => locker.Acquire(lockName));
                Assert.Equal(DistributedAppLockExceptionReason.LockAlreadyAcquired, ex.Reason);
            }
        }

        [Fact]
        public void Test_acquire_lock_with_timeout_works()
        {
            /*
            This test flow goes something like:
            1. acquire firstLock
            2. Start a new thread that immediately returns and waits for a while
            3. Try to acquire second lock, but be patient and wait for up to 20 secs while retrying
            4. thread unlocks first lock after 700 ms
            5. thread waits 500 ms
            6. now we expect the second call to acquire to have succeeded, assert that this is true
             */
            InMemoryDistributedAppLock firstLock = null;
            InMemoryDistributedAppLock secondLock = null;
            var lockName = "some-other-lock";

            var locker = new InMemoryDistributedAppLockProvider();
            firstLock = (InMemoryDistributedAppLock) locker.Acquire(lockName, TimeSpan.FromSeconds(1));
            Assert.True(firstLock.WasAcquiredInstantly);
            
            // Create task to dispose of lock at some point
            Task.Factory.StartNew(() =>
            {
                Task.Delay(TimeSpan.FromMilliseconds(700));
                // release the first lock
                firstLock.Dispose();

                // wait and the new lock should be acquired
                Task.Delay(TimeSpan.FromMilliseconds(500));
                Assert.Equal(lockName, secondLock.Name);
                Assert.True(secondLock.IsActive);
                Assert.False(secondLock.WasAcquiredInstantly);
                Assert.True(secondLock.TimeUsedToAcquire.Ticks > 0);
                secondLock.Dispose();
            });

            // this second lock now enters retry mode
            secondLock = (InMemoryDistributedAppLock) locker.Acquire(lockName, TimeSpan.FromSeconds(20));
        }

        [Fact]
        public void Test_that_is_active_is_false_when_disposed()
        {
            var provider = new InMemoryDistributedAppLockProvider();
            var theLock = provider.Acquire("this-is-my-lock");
            theLock.Dispose();
            Assert.False(theLock.IsActive);
        }

        [Fact]
        public async void Test_that_lock_is_released_on_exception_in_using_block()
        {
            var locker = new InMemoryDistributedAppLockProvider();
            var lockName = "is_released_on_exception";
            var lockWait = TimeSpan.FromSeconds(3);

            try
            {
                using (await locker.AcquireAsync(lockName, lockWait))
                {
                    throw new InvalidOperationException("oh shit");
                }
            }
            catch (Exception)
            {
                // nothing to do
            }
            
            // we should be able to acquire the same lock now
            using (await locker.AcquireAsync(lockName, lockWait))
            {
                // yay
            }
        }
    }
}