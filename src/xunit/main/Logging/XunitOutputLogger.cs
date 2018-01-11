using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace RapidCore.Xunit.Logging
{
    /// <summary>
    /// Actual log implementation for writing logs to the xunit <see cref="ITestOutputHelper"/>
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILogger" />
    public class XUnitOutputLogger : ILogger
    {
        private static readonly object LockObject = new object();
        internal readonly string CategoryName;
        private readonly ITestOutputHelper _output;

        public XUnitOutputLogger(string categoryName, ITestOutputHelper output)
        {
            CategoryName = categoryName;
            _output = output;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            lock (LockObject)
            {
                _output.WriteLine($"{DateTime.Now}\t[{logLevel.ToString()}]\t{eventId.Name}\t{CategoryName}\t{formatter(state, exception)}\t{exception}");
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }
    }
}
