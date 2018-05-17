using System;
using System.Data;
using System.Threading.Tasks;
using RapidCore.Locking;
using RapidCore.Threading;

namespace RapidCore.SqlServer.Locking
{
    public class SqlServerDistributedAppLockProvider : IDistributedAppLockProvider
    {
        private readonly Func<IDbConnection> _dbConnectionFactory;


        public SqlServerDistributedAppLockProvider(Func<IDbConnection> dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        
        public IDistributedAppLock Acquire(string lockName, TimeSpan? lockWaitTimeout = null, TimeSpan? lockAutoExpireTimeout = null)
        {
            return AcquireAsync(lockName, lockWaitTimeout, lockAutoExpireTimeout).AwaitSync();
        }

        public async Task<IDistributedAppLock> AcquireAsync(string lockName, TimeSpan? lockWaitTimeout = null, TimeSpan? lockAutoExpireTimeout = null)
        {
            var handle = new SqlServerDistributedAppLock(_dbConnectionFactory);
            return await handle.AcquireLockAsync(lockName, lockWaitTimeout, lockAutoExpireTimeout);
        }
    }
}