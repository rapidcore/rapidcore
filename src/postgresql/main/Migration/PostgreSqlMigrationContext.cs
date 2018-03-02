using Microsoft.Extensions.Logging;
using RapidCore.DependencyInjection;
using RapidCore.Migration;

namespace RapidCore.PostgreSql.Migration
{
    public class PostgreSqlMigrationContext : IMigrationContext
    {
        public virtual ILogger Logger { get; set; }
        public virtual IRapidContainerAdapter Container { get; set; }
        public virtual IMigrationEnvironment Environment { get; set; }
        public virtual IMigrationStorage Storage { get; set; }
        public virtual PostgreSqlConnectionProvider ConnectionProvider { get; set; }
    }
}
