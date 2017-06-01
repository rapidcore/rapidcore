using System;
using System.Threading.Tasks;
using FakeItEasy;
using RapidCore.Locking;
using RapidCore.Redis.Locking;
using StackExchange.Redis;
using Xunit;

namespace RapidCore.Redis.UnitTests.Locking
{
    public class RedisDistributedAppLockTest
    {
        [Fact]
        public async Task Does_acquire_lock_using_redisclient()
        {
            var lockName = "the-lock";
            var client = A.Fake<IDatabase>(o => o.Strict());
            var manager = A.Fake<IConnectionMultiplexer>(o => o.Strict());
            A.CallTo(() => manager.GetDatabase(A<int>.Ignored, A<object>.Ignored)).Returns(client);
            A.CallTo(() => client.LockTake(
                A<RedisKey>.That.Matches(str => str == lockName),
                A<RedisValue>.Ignored,
                A<TimeSpan>.Ignored,
                A<CommandFlags>.Ignored)).Returns(true);

            var handle = new RedisDistributedAppLock(manager);

            await handle.AcquireLockAsync(lockName);

            A.CallTo(() => client.LockTake(
                    A<RedisKey>.That.Matches(str => str == lockName),
                    A<RedisValue>.Ignored,
                    A<TimeSpan>.Ignored,
                    A<CommandFlags>.Ignored))
                .MustHaveHappened();

            Assert.Equal(lockName, handle.Name);
        }

        [Fact]
        public async Task Does_dispose_of_underlying_resources()
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

            var handle = new RedisDistributedAppLock(manager);

            using (await handle.AcquireLockAsync(lockName, TimeSpan.FromSeconds(2)))
            {
                // empty on purpose
            }

            // after using scope underlying lock must be released
            A.CallTo(() => client.LockRelease(
                A<RedisKey>.That.Matches(str => str == lockName),
                A<RedisValue>.Ignored,
                A<CommandFlags>.Ignored)).MustHaveHappened();
        }

        [Fact]
        public async Task Properly_traps_redis_client_exceptions()
        {
            var lockName = "the-lock";

            var client = A.Fake<IDatabase>();
            var manager = A.Fake<IConnectionMultiplexer>();
            A.CallTo(() => manager.GetDatabase(A<int>.Ignored, A<object>.Ignored)).Returns(client);

            A.CallTo(() => client.LockTake(
                 A<RedisKey>.That.Matches(str => str == lockName),
                 A<RedisValue>.Ignored,
                 A<TimeSpan>.Ignored,
                 A<CommandFlags>.Ignored)).Throws(new Exception("test is faking it!"));

            var handle = new RedisDistributedAppLock(manager);

            var ex = await Assert.ThrowsAsync<DistributedAppLockException>(
                async () => await handle.AcquireLockAsync(lockName, TimeSpan.FromSeconds(2)));
            Assert.Equal(DistributedAppLockExceptionReason.SeeInnerException, ex.Reason);
        }

        [Fact]
        public async Task Properly_traps_timeouts()
        {
            var lockName = "the-lock";

            var client = A.Fake<IDatabase>();
            var manager = A.Fake<IConnectionMultiplexer>();
            A.CallTo(() => manager.GetDatabase(A<int>.Ignored, A<object>.Ignored)).Returns(client);

            var exceptionThrownDuringTest = new TimeoutException("test is faking it!");
            A.CallTo(() => client.LockTake(
                 A<RedisKey>.That.Matches(str => str == lockName),
                 A<RedisValue>.Ignored,
                 A<TimeSpan>.Ignored,
                 A<CommandFlags>.Ignored)).Throws(exceptionThrownDuringTest);

            var handle = new RedisDistributedAppLock(manager);


            var ex = await Assert.ThrowsAsync<DistributedAppLockException>(
                async () => await handle.AcquireLockAsync(lockName, TimeSpan.FromSeconds(2)));
            Assert.Equal(DistributedAppLockExceptionReason.Timeout, ex.Reason);
            var innerEx = ex.InnerException;
            Assert.NotNull(innerEx);
            Assert.Equal(exceptionThrownDuringTest, innerEx);
        }

        [Fact]
        public async Task Properly_traps_other_exceptions()
        {
            var lockName = "the-lock";

            var client = A.Fake<IDatabase>();
            var manager = A.Fake<IConnectionMultiplexer>();
            A.CallTo(() => manager.GetDatabase(A<int>.Ignored, A<object>.Ignored)).Returns(client);

            A.CallTo(() => client.LockTake(
                 A<RedisKey>.That.Matches(str => str == lockName),
                 A<RedisValue>.Ignored,
                 A<TimeSpan>.Ignored,
                 A<CommandFlags>.Ignored)).Throws(new OperationCanceledException("test is faking it!"));

            var handle = new RedisDistributedAppLock(manager);

            var ex = await Assert.ThrowsAsync<DistributedAppLockException>(
                async () => await handle.AcquireLockAsync(lockName, TimeSpan.FromSeconds(2)));
            Assert.Equal(DistributedAppLockExceptionReason.SeeInnerException, ex.Reason);
            Assert.IsType<OperationCanceledException>(ex.InnerException);
        }
    }
}