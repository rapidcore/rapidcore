namespace RapidCore.SqlServer.Locking
{
    /// <summary>
    /// Configuration object for <see cref="SqlServerDistributedAppLock"/>
    /// </summary>
    public class SqlServerDistributedAppLockConfig
    {
        /// <summary>
        /// Whether the underlying database connection should be closed and disposed off when the lock is disposed
        /// </summary>
        public bool DisposeDbConnection { get; set; }
    }
}