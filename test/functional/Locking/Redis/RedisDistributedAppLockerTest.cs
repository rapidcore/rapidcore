using System;
using System.Threading.Tasks;
using RapidCore.Locking;
using RapidCore.Locking.Redis;
using StackExchange.Redis;
using Xunit;

namespace RapidCore.FunctionalTest.Locking.Redis
{
    public class RedisDistributedAppLockerTest
    {
        private readonly IConnectionMultiplexer _redisMuxer;
        private readonly string _hostname;

        public RedisDistributedAppLockerTest()
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
            var locker = new RedisDistributedAppLocker(_redisMuxer);
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
            var locker = new RedisDistributedAppLocker(_redisMuxer);

            using (locker.Acquire(lockName, TimeSpan.FromSeconds(1)))
            {
                var ex = Assert.Throws<DistributedAppLockException>(() => locker.Acquire(lockName));
                Assert.Equal(DistributedAppLockExceptionReason.LockAlreadyAcquired, ex.Reason);
            }
        }

        [Fact]
        public void Test_acquire_lock_with_timeout_works()
        {
            RedisDistributedAppLock firstLock = null;
            RedisDistributedAppLock secondLock = null;
            var lockName = "some-other-lock";
            // ensure that no stale keys are left
            _redisMuxer.GetDatabase().KeyDelete(lockName);
            
            var locker = new RedisDistributedAppLocker(_redisMuxer);
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
                Assert.True(secondLock.HasAcquiredLock);
                secondLock.Dispose();
            });

            // this second lock now enters retry mode
            secondLock = (RedisDistributedAppLock) locker.Acquire(lockName, TimeSpan.FromSeconds(20));

            
        }
    }
}