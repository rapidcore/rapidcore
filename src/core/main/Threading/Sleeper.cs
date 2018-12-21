using System;
using System.Threading.Tasks;

namespace RapidCore.Threading
{
    public class Sleeper
    {
        public virtual async Task SleepAsync(TimeSpan howLongToSleep)
        {
            await Task.Delay(howLongToSleep);
        }
    
        public virtual async Task SleepAsync(int howLongToSleepInMilliseconds)
        {
            await SleepAsync(TimeSpan.FromMilliseconds(howLongToSleepInMilliseconds));
        }
    }
}