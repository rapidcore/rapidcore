using System;
using System.Threading.Tasks;
using FakeItEasy;
using RapidCore.Locking.Redis;
using StackExchange.Redis;
using Xunit;

namespace RapidCore.UnitTests.Locking.Redis
{
    public class RedisDistributedAppLockerTest
    {
        [Fact]
        public void Test_does_create_new_redis_client()
        {
            var lockName = "the-lock";
            
            var client = A.Fake<IDatabase>();
            var manager = A.Fake<IConnectionMultiplexer>();
            A.CallTo(() => manager.GetDatabase(A<int>.Ignored, A<object>.Ignored)).Returns(client);
            A.CallTo(() => client.LockTake(
                A<RedisKey>.That.Matches(str => str == lockName),
                A<RedisValue>.Ignored,
                A<TimeSpan>.Ignored,
                A<CommandFlags>.Ignored)).Returns(true);
            var locker = new RedisDistributedAppLocker(manager);

            locker.Acquire(lockName);

            A.CallTo(() => manager.GetDatabase(A<int>.Ignored, A<object>.Ignored)).MustHaveHappened();
        }
        
        [Fact]
        public void Test_does_call_acquire_lock_on_lock_handle()
        {
            
            var lockName = "the-lock";
            
            var client = A.Fake<IDatabase>();
            var manager = A.Fake<IConnectionMultiplexer>();
            A.CallTo(() => manager.GetDatabase(A<int>.Ignored, A<object>.Ignored)).Returns(client);
            A.CallTo(() => client.LockTake(
                A<RedisKey>.That.Matches(str => str == lockName),
                A<RedisValue>.Ignored,
                A<TimeSpan>.Ignored,
                A<CommandFlags>.Ignored)).Returns(true);
            var locker = new RedisDistributedAppLocker(manager);

            locker.Acquire(lockName);

            A.CallTo(() => client.LockTake(
                A<RedisKey>.That.Matches(key => key == lockName),
                A<RedisValue>.Ignored,
                A<TimeSpan>.Ignored,
                A<CommandFlags>.Ignored

            )).MustHaveHappened();

        }
    }
}
