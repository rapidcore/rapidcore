# AsyncAwait


It can be very annoying to be stuck in synchronous code if you need to call something `async`. The error handling involved is annoying, because the `Task` will wrap errors in `AggregateException`.

`RapidCore.Threading` contains an extension method for `Task` and `Task<T>` called `AsyncAwait`.

This method will **synchronous** and will either return the result of the task, or catch the AggregateException and rethrow the actual exception (i.e. it unwraps the AggregateException).

#### Examples

- [How to use AsyncAwait](../Examples#test-calling-thirdpartythisismytime-with-the-correct-parameters)