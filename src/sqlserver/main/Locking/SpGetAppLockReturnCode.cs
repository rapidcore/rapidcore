namespace RapidCore.SqlServer.Locking
{
    public enum SpGetAppLockReturnCode
    {
        Granted = 0,
        
        GrantedAfterWait = 1,
        
        LockRequestTimeout = -1,
        
        LockRequestCanceled = -2,
        
        LockRequestIsDeadlockVictim = -3,
        
        CallOrParameterError = -999,
    }
}