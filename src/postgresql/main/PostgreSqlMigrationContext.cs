using System;
using Microsoft.Extensions.Logging;
using RapidCore.DependencyInjection;
using RapidCore.Migration;

namespace Rapidcore.Postgresql
{
    public class PostgreSqlMigrationContext : IMigrationContext
    {
        public ILogger Logger { get; set; }
        public IRapidContainerAdapter Container { get; set; }
        public IMigrationEnvironment Environment { get; set; }
        public IMigrationStorage Storage { get; set; }
        public PostgreSqlConnectionProvider ConnectionProvider { get; set; }
    }
}
