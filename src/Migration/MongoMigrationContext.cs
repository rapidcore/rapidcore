using Microsoft.Extensions.Logging;
using RapidCore.DependencyInjection;
using RapidCore.Migration;

namespace RapidCore.Mongo.Migration
{
    public class MongoMigrationContext : IMigrationContext
    {
        public ILogger Logger { get; set; }
        public IRapidContainerAdapter Container { get; set; }
        public IMigrationEnvironment Environment { get; set; }
        public IMigrationStorage Storage { get; set; }
        public ConnectionProvider ConnectionProvider { get; set; }
    }
}