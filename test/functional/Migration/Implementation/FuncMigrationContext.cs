using Microsoft.Extensions.Logging;
using RapidCore.DependencyInjection;
using RapidCore.Migration;

namespace RapidCore.FunctionalTests.Migration.Implementation
{
    public class FuncMigrationContext : IMigrationContext
    {
        public ILogger Logger { get; set; }
        public IRapidContainerAdapter Container { get; set; }
        public IMigrationEnvironment Environment { get; set; }
        public FuncMigrationDatabase Database { get; set; }
    }
}