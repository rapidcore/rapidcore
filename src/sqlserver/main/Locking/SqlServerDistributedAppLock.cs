using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using RapidCore.Locking;

namespace RapidCore.SqlServer.Locking
{
    public class SqlServerDistributedAppLock : IDistributedAppLock
    {
        private readonly Func<IDbConnection> _dbConnectionFactory;

        private IDbConnection _dbConnection;

        private bool _disposedValue;

        public SqlServerDistributedAppLock(Func<IDbConnection> dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public string Name { get; private set; }

        public bool IsActive { get; private set; }


        public async Task<IDistributedAppLock> AcquireLockAsync(
            string lockName,
            TimeSpan? lockWaitTimeout = null,
            TimeSpan? lockAutoExpireTimeout = null)
        {
            try
            {
                _dbConnection = _dbConnectionFactory();

                var timeoutProvided = lockWaitTimeout.HasValue;
                if (!timeoutProvided)
                {
                    lockWaitTimeout = TimeSpan.Zero;
                }

                if (lockAutoExpireTimeout.HasValue)
                {
                    throw new NotSupportedException("Sql AppLock does not support auto expiry.");
                }

                var parameters = new DynamicParameters();
                parameters.Add("@Resource", lockName);
                parameters.Add("@LockMode", "Exclusive");
                parameters.Add("@DbPrincipal", "public");
                parameters.Add("@LockOwner", "Session");
                parameters.Add("@LockTimeout", lockWaitTimeout.Value.TotalMilliseconds);
                parameters.Add("exitCode", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await _dbConnection.ExecuteAsync("sp_getapplock",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                var exitCode = parameters.Get<int>("exitCode");

                if (exitCode < (int) SpGetAppLockReturnCode.Granted)
                {
                    // acquire lock failure
                    if (exitCode == (int) SpGetAppLockReturnCode.LockRequestTimeout && lockWaitTimeout != TimeSpan.Zero)
                    {
                        throw new DistributedAppLockException($"Timeout while acquiring lock: '{lockName}'")
                        {
                            Reason = DistributedAppLockExceptionReason.Timeout
                        };
                    }

                    throw new DistributedAppLockException($"Unable to acquire lock: '{lockName}'")
                    {
                        Reason = DistributedAppLockExceptionReason.LockAlreadyAcquired
                    };
                }

                // lock is available, whop whop!
                IsActive = true;
                Name = lockName;
                return this;
            }
            catch (DistributedAppLockException)
            {
                throw; // simply rethrow these types of exceptions to avoid being trapped in the generic Exception catcher
            }
            catch (Exception rex)
            {
                var ex = new DistributedAppLockException($"Unable to acquire lock: '{lockName}'", rex)
                {
                    Reason = DistributedAppLockExceptionReason.SeeInnerException,
                };
                throw ex;
            }
        }

        /// <summary>
        /// Determines whether the current lock instance is <see cref="IsActive"/> and has a name that matches the given
        /// parameter
        /// </summary>
        /// <param name="name">The name of the lock to asssert that is currently taken</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ThrowIfNotActiveWithGivenName(string name)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException(
                    $"Lock precondition mismatch, required IsActive=true with name '{name}' but IsActive=false with name '{this.Name}'");
            }

            if (!Name.Equals(name))
            {
                throw new InvalidOperationException(
                    $"Lock precondition mismatch, required IsActive=true with name '{name}' but IsActive=true with name '{this.Name}'");
            }
        }


        /// <summary>
        /// Dispose of the lock instance and release the lock with the underlying Redis system
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                // DISPOSE THE UNDERLYING SQLSERVER STUFF
                var parameters = new DynamicParameters();
                parameters.Add("@Resource", Name);
                parameters.Add("@LockOwner", "Session");
                parameters.Add("exitCode", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                _dbConnection.Execute("sp_releaseapplock",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                var exitCode = parameters.Get<int>("exitCode");

                if (exitCode == (int) SpGetAppLockReturnCode.CallOrParameterError)
                {
                    // TODO what to do if we are passing invalid data to SQL server?
                }

                IsActive = false;
                Name = null;
            }

            _disposedValue = true;
        }
    }
}