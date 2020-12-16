using System;
using FakeItEasy;
using RapidCore.Redis.Locking;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Xunit;

namespace UnitTests.Redis.Locking
{
    public class RedisDistributedAppLockProviderTest
    {
        [Fact]
        public void Test_does_create_new_redis_client()
        {
            var lockName = "the-lock";
            
            var client = A.Fake<IDatabase>();
            var pool = A.Fake<IRedisCacheConnectionPoolManager>();
            var manager = A.Fake<IConnectionMultiplexer>();
            
            A.CallTo(() => pool.GetConnection()).Returns(manager);
            A.CallTo(() => manager.GetDatabase(A<int>.Ignored, A<object>.Ignored)).Returns(client);
            A.CallTo(() => client.LockTakeAsync(
                A<RedisKey>.That.Matches(str => str == lockName),
                A<RedisValue>.Ignored,
                A<TimeSpan>.Ignored,
                A<CommandFlags>.Ignored)).Returns(true);
            var locker = new RedisDistributedAppLockProvider(pool);

            locker.Acquire(lockName);

            A.CallTo(() => pool.GetConnection()).MustHaveHappened();
            A.CallTo(() => manager.GetDatabase(A<int>.Ignored, A<object>.Ignored)).MustHaveHappened();
        }
        
        [Fact]
        public void Test_does_call_acquire_lock_on_lock_handle()
        {
            
            var lockName = "the-lock";
            
            var client = A.Fake<IDatabase>();
            var pool = A.Fake<IRedisCacheConnectionPoolManager>();
            var manager = A.Fake<IConnectionMultiplexer>();
            
            A.CallTo(() => pool.GetConnection()).Returns(manager);
            A.CallTo(() => manager.GetDatabase(A<int>.Ignored, A<object>.Ignored)).Returns(client);
            A.CallTo(() => client.LockTakeAsync(
                A<RedisKey>.That.Matches(str => str == lockName),
                A<RedisValue>.Ignored,
                A<TimeSpan>.Ignored,
                A<CommandFlags>.Ignored)).Returns(true);
            var locker = new RedisDistributedAppLockProvider(pool);

            locker.Acquire(lockName);

            A.CallTo(() => client.LockTakeAsync(
                A<RedisKey>.That.Matches(key => key == lockName),
                A<RedisValue>.Ignored,
                A<TimeSpan>.Ignored,
                A<CommandFlags>.Ignored

            )).MustHaveHappened();

        }
    }
}
