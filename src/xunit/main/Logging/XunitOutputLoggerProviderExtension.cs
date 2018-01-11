using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace RapidCore.Xunit.Logging
{
    public static class XunitOutputLoggerProviderExtension
    {
        /// <summary>
        /// Adds the xunit output to the logging factory.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        public static ILoggerFactory AddXunitOutput(this ILoggerFactory factory, ITestOutputHelper output)
        {
            factory.AddProvider((ILoggerProvider)new XUnitOutputLoggerProvider(output));
            return factory;
        }
    }
}
