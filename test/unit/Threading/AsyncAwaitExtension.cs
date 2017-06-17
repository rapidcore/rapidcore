using System;
using System.Threading.Tasks;
using RapidCore.Threading;
using Xunit;

namespace RapidCore.UnitTests.Threading
{
    public class AsyncAwaitExtension
    {
        public AsyncAwaitExtension()
        {
            
        }

        [Fact]
        public void AsyncAwait_can_wait_for_op_with_result()
        {
            var res = FriendlyAsync().AwaitSync();
            Assert.Equal(42, res);
        }

        [Fact]
        public void AsyncAwait_does_get_root_exception_when_stuff_dies()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => UnfriendlyAsync().AwaitSync());
            Assert.Equal("Test threw this", ex.Message);
        }

        [Fact]
        public void AsyncAwait_can_wait_for_op_with_no_result()
        {
            FriendlyWithoutReturnType().AwaitSync();
            Assert.True(true);
        }

        [Fact]
        public void AsyncAwait_does_get_root_exception_on_non_return_type_tasks()
        {
            var ex = Assert.Throws<ArgumentException>(() => UnfriendlyWithoutReturnTypeAsync().AwaitSync());
            Assert.Equal("Arguments are thrown", ex.Message);
        }

        private async Task<int> FriendlyAsync()
        {
            await Task.Delay(10);
            return 42;
        }

        private async Task FriendlyWithoutReturnType()
        {
            await Task.Delay(10);
            return;
        }

        private async Task<int> UnfriendlyAsync()
        {
            await Task.Delay(10);
            throw new InvalidOperationException("Test threw this");
        }

        private async Task UnfriendlyWithoutReturnTypeAsync()
        {
            await Task.Delay(10);
            throw new ArgumentException("Arguments are thrown");
        }
    }
}