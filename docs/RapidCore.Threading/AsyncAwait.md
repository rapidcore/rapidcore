# AsyncAwait

It can be very annoying to be stuck in synchronous code if you need to call something `async`. The error handling involved is annoying, because the `Task` will wrap errors in `AggregateException`.

`RapidCore.Threading` contains an extension method for `Task` and `Task<T>` called `AsyncAwait`.

This method will **synchronous** and will either return the result of the task, or catch the AggregateException and rethrow the actual exception (i.e. it unwraps the AggregateException).


## Using it

```csharp
using RapidCore.Threading;

public class OldSchool
{
    public SomeResult HasToBeSynchronous()
    {
        try
        {
            var someResult = client.CallSomethingAsync().AwaitSync();
            return someResult;
        }
        catch (Exception ex)
        {
            // ex is the actual exception (e.g. HttpMessageException) and _NOT_ an AggregateException
        }
    }
}
```
