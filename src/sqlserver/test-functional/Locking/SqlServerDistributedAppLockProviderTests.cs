using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
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
        private readonly SqlConnection _connection;
        private readonly string _connectionString;

        public SqlServerDistributedAppLockProviderTests()
        {
            var env = new EnvironmentVariables();
            _connectionString = env.Get("SQL_SERVER_CONNECTION",
                "Server=localhost,1433; Trusted_Connection=False; User=sa; Password=sql-s3rv3r%");

            _connection = new SqlConnection(_connectionString);

            _connectionFactory = () =>
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                return _connection;
            };
        }

        [Fact]
        public async Task Acquire_lock_multiple_times_work()
        {
            const string lockName = "my-lock";

            var locker = new SqlServerDistributedAppLockProvider(_connectionFactory,
                new SqlServerDistributedAppLockConfig
                {
                    DisposeDbConnection = false
                });

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

            var locker = new SqlServerDistributedAppLockProvider(_connectionFactory);

            using (locker.Acquire(lockName, TimeSpan.FromSeconds(1)))
            {
                var ex = Assert.Throws<DistributedAppLockException>(() =>
                {
                    // we have to create a new connection here as taking the lock twice via the same connection is OK
                    var otherLocker = new SqlServerDistributedAppLockProvider(() =>
                    {
                        var conn = new SqlConnection(_connectionString);
                        conn.Open();

                        return conn;
                    });
                    return otherLocker.Acquire(lockName);
                });
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

            var threadStarted = false;
            // Create task to dispose of lock at some point
            Task.Factory.StartNew(() =>
            {
                threadStarted = true;
                
                // Wait 700 ms to allow the main thread to start acquiring second lock
                var timeStart = DateTime.UtcNow;
                while (DateTime.UtcNow.Subtract(timeStart).TotalMilliseconds < 700)
                {
                    // Do nothing
                }
                
                // release the first lock
                firstLock.Dispose();

                // wait and the new lock should be acquired
                Task.Delay(TimeSpan.FromMilliseconds(500));
                Assert.Equal(lockName, secondLock.Name);
                Assert.True(secondLock.IsActive);
                secondLock.Dispose();
            });
            
            // Thread creation can take some time
            // This waits for the thread to be created
            SpinWait.SpinUntil(() => threadStarted);
            
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

        [Fact]
        public async Task Default_behavior_disposes_connection()
        {
            var hasBeenDisposed = false;
            _connection.Disposed += (sender, args) => { hasBeenDisposed = true; };

            var provider = new SqlServerDistributedAppLockProvider(_connectionFactory);
            var handle = await provider.AcquireAsync("some-lock");

            handle.Dispose();

            await Task.Delay(50);

            Assert.True(hasBeenDisposed);
            Assert.Equal(ConnectionState.Closed, _connection.State);
        }

        [Fact]
        public async Task Configured_behavior_leaves_connection_intact()
        {
            var hasBeenDisposed = false;
            _connection.Disposed += (sender, args) => { hasBeenDisposed = true; };

            var provider = new SqlServerDistributedAppLockProvider(_connectionFactory,
                new SqlServerDistributedAppLockConfig
                {
                    DisposeDbConnection = false
                });

            var handle = await provider.AcquireAsync("some-lock");

            handle.Dispose();

            await Task.Delay(50);

            Assert.False(hasBeenDisposed);
            Assert.Equal(ConnectionState.Open, _connection.State);
        }


        [Fact]
        public async Task Throws_if_given_connection_is_not_open()
        {
            var locker = new SqlServerDistributedAppLockProvider(() => new SqlConnection(_connectionString));
            var ex = await Assert.ThrowsAsync<DistributedAppLockException>(async () => { await locker.AcquireAsync("TheLock"); });
            var arEx = Assert.IsAssignableFrom<ArgumentException>(ex.InnerException);
            
            Assert.Equal("dbConnection", arEx.ParamName);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}