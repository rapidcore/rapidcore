using System;
using System.Threading.Tasks;

namespace RapidCore.Threading
{
    public static class AsyncAwaitExtension
    {
        /// <summary>
        /// Extension method that can wait for the async execution of a task and return the result
        /// </summary>
        /// <remarks>
        /// This extension method is particularly useful if you have async code that must be excuted in a
        /// non async method that cannot be decorated async - like Program.cs or older code / code that must adhere
        /// no a non async interface
        ///
        /// It will also ensure that if exceptions are being thrown that they are flattened such that the "real" exception is thrown
        /// rather than the usual <see cref="AggregateException" /> thrown by the Task system
        /// </remarks>
        /// <param name="awaitable">The async execution that should be awaited in a blocking manner</param>
        /// <returns>The result of the async operation</returns>
        public static T AwaitSync<T>(this Task<T> awaitable)
        {
            try
            {
                awaitable.ConfigureAwait(false);
                var result = awaitable.Result;
                return result;
            }
            catch (AggregateException ae)
            {
                // rethrow the Aggregate exception but flatten it
                throw ae.Flatten().InnerException;
            }
        }

        /// <summary>
        /// Extension method that can wait for the async execution of a task and return void
        /// </summary>
        /// <remarks>
        /// This extension method is particularly useful if you have async code that must be excuted in a
        /// non async method that cannot be decorated async - like Program.cs or older code / code that must adhere
        /// no a non async interface
        ///
        /// It will also ensure that if exceptions are being thrown that they are flattened such that the "real" exception is thrown
        /// rather than the usual <see cref="AggregateException" /> thrown by the Task system
        /// </remarks>
        /// <param name="awaitable">The async execution that should be awaited in a blocking manner</param>
        /// <returns>Nothing</returns>
        public static void AwaitSync(this Task awaitable)
        {
            try
            {
                awaitable.ConfigureAwait(false);
                awaitable.Wait();
                return;
            }
            catch (AggregateException ae)
            {
                // rethrow the Aggregate exception but flatten it
                throw ae.Flatten().InnerException;
            }
        }
    }
}