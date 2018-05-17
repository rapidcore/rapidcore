using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using RapidCore.Environment;
using RapidCore.Locking;
using RapidCore.SqlServer.Locking;
using Xunit;

namespace functionaltests.Locking
{
    public class SqlServerDistributedAppLockProviderTests : IDisposable

    {
        private readonly Func<IDbConnection> _connectionFactory;
        private SqlConnection _connection;

        public SqlServerDistributedAppLockProviderTests()
        {
            var env = new EnvironmentVariables();
            var connectionString = env.Get("SQL_SERVER_CONNECTION",
                "Server=localhost,1433; Trusted_Connection=False; User=sa; Password=sql-s3rv3r%");

            _connectionFactory = () =>
            {
                _connection = new SqlConnection(connectionString);
                _connection.Open();
                return _connection;
            };
        }

        [Fact]
        public async Task Acquire_lock_multiple_times_work()
        {
            const string lockName = "my-lock";

            var locker = new SqlServerDistributedAppLockProvider(_connectionFactory);

            using (await locker.AcquireAsync(lockName))
            {
                // mutual exclusion scope here
            }

            using (await locker.AcquireAsync(lockName))
            {
                // mutual exclusion scope here
            }

            using (await locker.AcquireAsync(lockName))
            {
                // mutual exclusion scope here
            }
        }

        [Fact]
        public void Acquire_sync_also_works()
        {
            const string lockName = "my-lock";
            var locker = new SqlServerDistributedAppLockProvider(_connectionFactory);

            using (locker.Acquire(lockName))
            {
                // mutual exclusion scope here
            }
        }
        
        [Fact]
        public void Cannot_acquire_lock_twice()
        {
            const string lockName = "second-lock";
            // ensure that no stale keys are left
            
            var locker = new SqlServerDistributedAppLockProvider(_connectionFactory);

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
            SqlServerDistributedAppLock firstLock = null;
            SqlServerDistributedAppLock secondLock = null;
            var lockName = "some-other-lock";
            
            var locker = new SqlServerDistributedAppLockProvider(_connectionFactory);
            firstLock = (SqlServerDistributedAppLock) locker.Acquire(lockName, TimeSpan.FromSeconds(1));

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
            secondLock = (SqlServerDistributedAppLock) locker.Acquire(lockName, TimeSpan.FromSeconds(20));
        }
        
        [Fact]
        public void Test_that_is_acquired_is_false_when_disposed()
        {
            var provider = new SqlServerDistributedAppLockProvider(_connectionFactory);
            var theLock = provider.Acquire("this-is-my-lock");
            theLock.Dispose();
            Assert.False(theLock.IsActive);
        }
        
        [Fact]
        public async Task Acquire_with_auto_expiry_throws()
        {
            var ex = await Assert.ThrowsAsync<DistributedAppLockException>(async () =>
            {
                var locker = new SqlServerDistributedAppLockProvider(_connectionFactory);
                await locker.AcquireAsync("theLock", TimeSpan.Zero, TimeSpan.MaxValue);
            });

            Assert.IsAssignableFrom<NotSupportedException>(ex.InnerException);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}