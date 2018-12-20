# Distributed app locks

When you need to ensure that only 1 process (across all of your instances or services) is working on some resource, you need some form of distributed app lock. In `RapidCore` this is defined in `RapidCore.Locking.IDistributedAppLockProvider` and `RapidCore.Locking.IDistributedAppLock`.

You can implement your own or use one the implementations available in the RapidCore packages with dependencies:

- Noop in `RapidCore` for when you really do not care, but the framework requires it
- Redis in in the package `RapidCore.Redis`
- SqlServer in the package [`RapidCore.SqlServer`](../SqlServer/Locking.md)

#### Examples

- [Taking a lock](../Examples#taking-a-lock)
- [Handling errors](../Examples#handling-errors)