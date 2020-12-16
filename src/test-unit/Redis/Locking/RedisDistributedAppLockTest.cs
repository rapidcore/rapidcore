using System;
using System.Threading.Tasks;
using FakeItEasy;
using RapidCore.Locking;
using RapidCore.Redis.Locking;
using StackExchange.Redis;
using Xunit;

namespace UnitTests.Redis.Locking
{
    public class RedisDistributedAppLockTest
    {
        private readonly Random _rng;
        private readonly string _defaultLockName;
        private IDatabase _client;
        private IConnectionMultiplexer _manager;
        public RedisDistributedAppLockTest()
        {
            _rng = new Random();
            _defaultLockName = "the-lock";
            
            _client = A.Fake<IDatabase>(o => o.Strict());
            _manager = A.Fake<IConnectionMultiplexer>(o => o.Strict());
            A.CallTo(() => _manager.GetDatabase(A<int>.Ignored, A<object>.Ignored)).Returns(_client);
            A.CallTo(() => _client.LockTakeAsync(
                A<RedisKey>.That.Matches(str => str == _defaultLockName),
                A<RedisValue>.Ignored,
                A<TimeSpan>.Ignored,
                A<CommandFlags>.Ignored)).Returns(true);
            A.CallTo(() => _client.LockRelease(
                A<RedisKey>.That.Matches(str => str == _defaultLockName),
                A<RedisValue>.Ignored,
                A<CommandFlags>.Ignored
            )).Returns(true);
        }
        [Fact]
        public async Task Does_acquire_lock_using_redisclient()
        {
            var handle = new RedisDistributedAppLock(_manager, _rng);
            await handle.AcquireLockAsync(_defaultLockName);

            A.CallTo(() => _client.LockTakeAsync(
                    A<RedisKey>.That.Matches(str => str == _defaultLockName),
                    A<RedisValue>.Ignored,
                    A<TimeSpan>.Ignored,
                    A<CommandFlags>.Ignored))
                .MustHaveHappened();

            Assert.Equal(_defaultLockName, handle.Name);
        }

        [Fact]
        public async Task Does_acquire_lock_with_ttl_if_passed()
        {
            var handle = new RedisDistributedAppLock(_manager, _rng);
            await handle.AcquireLockAsync(_defaultLockName, TimeSpan.MaxValue, TimeSpan.FromSeconds(2));
            
            A.CallTo(() => _client.LockTakeAsync(
                    A<RedisKey>.That.Matches(str => str == _defaultLockName),
                    A<RedisValue>.Ignored,
                    TimeSpan.FromSeconds(2),
                    A<CommandFlags>.Ignored))
                .MustHaveHappened();

            Assert.Equal(_defaultLockName, handle.Name);

        }
        [Fact]
        public async Task Does_dispose_of_underlying_resources()
        {
            var handle = new RedisDistributedAppLock(_manager, _rng);

            using (await handle.AcquireLockAsync(_defaultLockName, TimeSpan.FromSeconds(2)))
            {
                // empty on purpose
            }

            // after using scope underlying lock must be released
            A.CallTo(() => _client.LockRelease(
                A<RedisKey>.That.Matches(str => str == _defaultLockName),
                A<RedisValue>.Ignored,
                A<CommandFlags>.Ignored)).MustHaveHappened();
        }

        [Fact]
        public void Does_throw_if_lock_not_active()
        {
            var handle = new RedisDistributedAppLock(_manager, _rng);
            var ex = Assert.Throws<InvalidOperationException>(() => handle.ThrowIfNotActiveWithGivenName(_defaultLockName));
            Assert.Equal(
                $"Lock precondition mismatch, required IsActive=true with name '{_defaultLockName}' but IsActive=false with name ''",
                ex.Message);
        }
        
        [Fact]
        public void Does_throw_if_lock_name_does_not_match()
        {
            var handle = new RedisDistributedAppLock(_manager, _rng);
            var ex = Assert.Throws<InvalidOperationException>(() => handle.ThrowIfNotActiveWithGivenName("this-is-the-wrong-name"));
        }
        
        [Fact]
        public async Task Does_not_throw_when_name_matches_and_lock_is_active()
        {
            using (var handle = await new RedisDistributedAppLock(_manager, _rng).AcquireLockAsync(_defaultLockName))
            {
                handle.ThrowIfNotActiveWithGivenName(_defaultLockName);
            }
        }
        
        [Fact]
        public async Task Properly_traps_redis_client_exceptions()
        {
            var lockName = "the-lock";

            var client = A.Fake<IDatabase>();
            var manager = A.Fake<IConnectionMultiplexer>();
            A.CallTo(() => manager.GetDatabase(A<int>.Ignored, A<object>.Ignored)).Returns(client);

            A.CallTo(() => client.LockTakeAsync(
                 A<RedisKey>.That.Matches(str => str == lockName),
                 A<RedisValue>.Ignored,
                 A<TimeSpan>.Ignored,
                 A<CommandFlags>.Ignored)).Throws(new Exception("test is faking it!"));

            var handle = new RedisDistributedAppLock(manager, _rng);

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
            A.CallTo(() => client.LockTakeAsync(
                 A<RedisKey>.That.Matches(str => str == lockName),
                 A<RedisValue>.Ignored,
                 A<TimeSpan>.Ignored,
                 A<CommandFlags>.Ignored)).Throws(exceptionThrownDuringTest);

            var handle = new RedisDistributedAppLock(manager, _rng);


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

            A.CallTo(() => client.LockTakeAsync(
                 A<RedisKey>.That.Matches(str => str == lockName),
                 A<RedisValue>.Ignored,
                 A<TimeSpan>.Ignored,
                 A<CommandFlags>.Ignored)).Throws(new OperationCanceledException("test is faking it!"));

            var handle = new RedisDistributedAppLock(manager, _rng);

            var ex = await Assert.ThrowsAsync<DistributedAppLockException>(
                async () => await handle.AcquireLockAsync(lockName, TimeSpan.FromSeconds(2)));
            Assert.Equal(DistributedAppLockExceptionReason.SeeInnerException, ex.Reason);
            Assert.IsType<OperationCanceledException>(ex.InnerException);
        }
    }
}