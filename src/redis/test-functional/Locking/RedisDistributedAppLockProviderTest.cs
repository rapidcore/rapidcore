using System;
using System.Threading.Tasks;
using RapidCore.Locking;
using RapidCore.Redis.Locking;
using StackExchange.Redis;
using Xunit;

namespace RapidCore.Redis.FunctionalTest.Locking
{
    public class RedisDistributedAppLockProviderTest
    {
        private readonly IConnectionMultiplexer _redisMuxer;
        private readonly string _hostname;

        public RedisDistributedAppLockProviderTest()
        {
            _hostname = "127.0.0.1:6379";
            _redisMuxer = ConnectionMultiplexer.Connect(_hostname);
        }

        [Fact]
        public void Test_using_multiple_times_work()
        {
            var lockName = "first-lock";
            // ensure that no stale keys are left
            _redisMuxer.GetDatabase().KeyDelete(lockName);
            var locker = new RedisDistributedAppLockProvider(_redisMuxer);
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
            // ensure that no stale keys are left
            _redisMuxer.GetDatabase().KeyDelete(lockName);
            var locker = new RedisDistributedAppLockProvider(_redisMuxer);

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
            RedisDistributedAppLock firstLock = null;
            RedisDistributedAppLock secondLock = null;
            var lockName = "some-other-lock";
            // ensure that no stale keys are left
            _redisMuxer.GetDatabase().KeyDelete(lockName);

            var locker = new RedisDistributedAppLockProvider(_redisMuxer);
            firstLock = (RedisDistributedAppLock) locker.Acquire(lockName, TimeSpan.FromSeconds(1));

            // Create task to dispose of loclk at some point
            Task.Factory.StartNew(() =>
            {
                Task.Delay(TimeSpan.FromMilliseconds(700));
                // release the first lock
                firstLock.Dispose();

                // wait and the new lock should be acquired
                Task.Delay(TimeSpan.FromMilliseconds(500));
                Assert.Equal(lockName, secondLock.Name);
                Assert.True(secondLock.IsActive);
                secondLock.Dispose();
            });

            // this second lock now enters retry mode
            secondLock = (RedisDistributedAppLock) locker.Acquire(lockName, TimeSpan.FromSeconds(20));
        }

        [Fact]
        public void Test_that_is_acquired_is_false_when_disposed()
        {
            var provider = new RedisDistributedAppLockProvider(_redisMuxer);
            var theLock = provider.Acquire("this-is-my-lock");
            theLock.Dispose();
            Assert.False(theLock.IsActive);
        }

        [Fact]
        public void Test_that_auto_expire_on_lock_works()
        {
            /*
           This test flow goes something like:
           1. acquire firstLock that auto-expires after 2 seconds 
           2. Start a new thread that will immediately sleep for 3 seconds to allow the auto-expire to kick in
           3. Try to acquire second lock which should work after 3 seconds as the lock has to be automatically released
           4. first thread unlocks after 3 seconds and then we can assert that our lock-states are correct
            */
            RedisDistributedAppLock firstLock = null;
            RedisDistributedAppLock secondLock = null;
            var lockName = "some-other-lock-that-expire";
            // ensure that no stale keys are left
            _redisMuxer.GetDatabase().KeyDelete(lockName);

            var locker = new RedisDistributedAppLockProvider(_redisMuxer);
            firstLock = (RedisDistributedAppLock) locker.Acquire(lockName, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));

            // Create task to dispose of lock at some point
            Task.Factory.StartNew(() =>
            {
                // delay for a bit more than the auto-expire, and we should be good to go.
                Task.Delay(TimeSpan.FromMilliseconds(3000));
                Assert.Equal(lockName, secondLock.Name);
                Assert.True(secondLock.IsActive);
                Assert.False(firstLock.IsActive);
                secondLock.Dispose();
                firstLock.Dispose();
            });

            // this second lock now enters retry mode
            secondLock = (RedisDistributedAppLock) locker.Acquire(lockName, TimeSpan.FromSeconds(20));
        }
    }
}