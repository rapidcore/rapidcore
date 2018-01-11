namespace RapidCore.Locking
{
    public enum DistributedAppLockExceptionReason
    {
        /// <summary>
        /// Status to use when the lock is acquired by someone else
        /// </summary>
        LockAlreadyAcquired,

        /// <summary>
        /// Timeout acquiring the lock, i.e someone else has acquired it
        /// </summary>
        Timeout,

        /// <summary>
        /// Something horrible happened - check the inner exception for details
        /// </summary>
        SeeInnerException
    }
}