# Examples

## How to use AsyncAwait

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