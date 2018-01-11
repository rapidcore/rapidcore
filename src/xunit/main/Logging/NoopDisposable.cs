using System;

namespace RapidCore.Xunit.Logging
{
    public class NoopDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}