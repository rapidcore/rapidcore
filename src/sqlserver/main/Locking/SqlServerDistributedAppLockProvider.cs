using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using RapidCore.Locking;
using RapidCore.Threading;

namespace RapidCore.SqlServer.Locking
{
    public class SqlServerDistributedAppLockProvider : IDistributedAppLockProvider
    {
        private readonly Func<IDbConnection> _dbConnectionFactory;
        private readonly SqlServerDistributedAppLockConfig _config;


        /// <summary>
        /// Creates a new instance of a provider that hands out <see cref="SqlServerDistributedAppLock"/> instances
        /// </summary>
        /// <remarks>
        /// Please note that using this constructor defaults to managing the connections returned by the factory which
        /// means that they will be closed and disposed when the lock is disposed off. If this is not the intended
        /// behavior please use the overloaded constructor and provide a configuration object
        /// </remarks>
        /// <param name="dbConnectionFactory">The Func for returning new OPEN database connections</param>
        public SqlServerDistributedAppLockProvider(Func<IDbConnection> dbConnectionFactory) : this(dbConnectionFactory,
            new SqlServerDistributedAppLockConfig
            {
                DisposeDbConnection = true
            })
        {
        }

        /// <summary>
        /// Creates a new instance of a provider that hands out <see cref="SqlServerDistributedAppLock"/> instances
        /// </summary>
        /// <param name="dbConnectionFactory">The Func for returning new OPEN database connections</param>
        /// <param name="config">Configuration object to alter the behavior of the lock provider, i.e disabling db connection management</param>
        public SqlServerDistributedAppLockProvider(
            Func<IDbConnection> dbConnectionFactory,
            SqlServerDistributedAppLockConfig config)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _config = config;
        }

        
        /// <summary>
        /// Acquire a new lock synchronously
        /// </summary>
        /// <remarks>
        /// Please note that passing a <param name="lockAutoExpireTimeout"></param> value is not supported and will result
        /// in thrown <see cref="NotSupportedException"/>s
        /// </remarks>
        /// <param name="lockName">The name of the lock</param>
        /// <param name="lockWaitTimeout">The amount of time to wait for the acquicution of the lock</param>
        /// <param name="lockAutoExpireTimeout">Not supported</param>
        /// <returns></returns>
        public IDistributedAppLock Acquire(string lockName, TimeSpan? lockWaitTimeout = null,
            TimeSpan? lockAutoExpireTimeout = null)
        {
            return AcquireAsync(lockName, lockWaitTimeout, lockAutoExpireTimeout).AwaitSync();
        }

        /// <summary>
        /// Acquire a lock asyncrhonosouly
        /// </summary>
        /// <remarks>
        /// Please note that passing a <param name="lockAutoExpireTimeout"></param> value is not supported and will result
        /// in thrown <see cref="NotSupportedException"/>s
        /// </remarks>
        /// <param name="lockName">The name of the lock</param>
        /// <param name="lockWaitTimeout">The amount of time to wait for the acquicution of the lock</param>
        /// <param name="lockAutoExpireTimeout">Not supported</param>
        /// <returns></returns>
        public async Task<IDistributedAppLock> AcquireAsync(string lockName, TimeSpan? lockWaitTimeout = null,
            TimeSpan? lockAutoExpireTimeout = null)
        {
            var handle = new SqlServerDistributedAppLock(_dbConnectionFactory, _config);
            return await handle.AcquireLockAsync(lockName, lockWaitTimeout, lockAutoExpireTimeout);
        }
    }
}