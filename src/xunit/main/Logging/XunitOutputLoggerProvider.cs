using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace RapidCore.Xunit.Logging
{

    /// <summary>
    /// A logger provider for the xunit output wrtier
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILoggerProvider" />
    public class XUnitOutputLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _output;

        public XUnitOutputLoggerProvider(ITestOutputHelper output)
        {
            _output = output;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitOutputLogger(categoryName, _output);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
