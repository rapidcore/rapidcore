# Distributed app locks

When you need to ensure that only 1 process (across all of your instances or services) is working on some resource, you need some form of distributed app lock. In `RapidCore` this is defined in `RapidCore.Locking.IDistributedAppLockProvider` and `RapidCore.Locking.IDistributedAppLock`.

You can implement your own or use one the implementations available in the RapidCore packages with dependencies:

- Noop in `RapidCore` for when you really do not care, but the framework requires it
- Redis in in the package `RapidCore.Redis`
- SqlServer in the package [`RapidCore.SqlServer`](../SqlServer/Locking.md)

## Taking a lock

```csharp
using RapidCore.Locking;

public class Worker
{
    private readonly IDistributedAppLockProvider appLockProvider;

    public Worker(IDistributedAppLockProvider appLockProvider)
    {
        this.appLockProvider = appLockProvider;
    }

    public async Task WorkOnSomethingSensitiveAsync()
    {
        /**
         * These are both optional, but default values
         * depend on the concrete implementation!
         */
        var howLongBeforeGivingUp = TimeSpan.FromSeconds(10);
        var howLongBeforeLockAutoExpires = TimeSpan.FromSeconds(60);

        using (var appLock = await appLockProvider.AcquireAsync("lock name", howLongBeforeGivingUp, howLongBeforeLockAutoExpires)) // there is also a synchronous version
        {
            // work on the sensitive resource
        }
    }
}
```

## Handling errors

The most typical error is not being able to acquire the lock, because someone else already have it and they do not complete before you give up waiting.

Otherwise, the errors will mostly be network related.

```csharp
using RapidCore.Locking;

public class Worker
{
    private readonly IDistributedAppLockProvider appLockProvider;

    public Worker(IDistributedAppLockProvider appLockProvider)
    {
        this.appLockProvider = appLockProvider;
    }

    public async Task WorkOnSomethingSensitiveAsync()
    {
        /**
         * These are both optional, but default values
         * depend on the concrete implementation!
         */
        var howLongBeforeGivingUp = TimeSpan.FromSeconds(10);
        var howLongBeforeLockAutoExpires = TimeSpan.FromSeconds(60);

        try
        {
            using (var appLock = await appLockProvider.AcquireAsync("lock name", howLongBeforeGivingUp, howLongBeforeLockAutoExpires))
            {
                // work on the sensitive resource
            }
        }
        catch (DistributedAppLockException ex)
        {
            /**
             * LockAlreadyAcquired = Lock is acquired by someone else
             * Timeout = Timeout acquiring the lock, i.e someone else has acquired it
             * SeeInnerException = Something horrible happened - check the inner exception for details
             */
            Console.WriteLine(ex.Reason);
        }
    }
}
```
