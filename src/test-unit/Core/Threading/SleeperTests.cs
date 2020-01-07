using System;
using System.Diagnostics;
using System.Threading.Tasks;
using RapidCore.Threading;
using Xunit;

namespace UnitTests.Core.Threading
{
    public class SleeperTests
    {

        private readonly Sleeper sleeper;
        private readonly Stopwatch stopwatch;

        public SleeperTests()
        {
            sleeper = new Sleeper();
            stopwatch = new Stopwatch();
        }

        [Theory]
        [InlineData(500)]
        [InlineData(1000)]
        public async Task SleepAsync__withTimeSpan_sleepsExpectedAmount(int sleep)
        {
            stopwatch.Start();
            await sleeper.SleepAsync(TimeSpan.FromMilliseconds(sleep));
            stopwatch.Stop();
            
            Assert.InRange(stopwatch.ElapsedMilliseconds, sleep - 50, long.MaxValue);
        }
        
        [Theory]
        [InlineData(500)]
        [InlineData(1000)]
        public async Task SleepAsync_withMilliseconds_sleepsExpectedAmount(int sleep)
        {
            stopwatch.Start();
            await sleeper.SleepAsync(sleep);
            stopwatch.Stop();

            Assert.InRange(stopwatch.ElapsedMilliseconds, sleep - 50, long.MaxValue);
        }
        
    }
}