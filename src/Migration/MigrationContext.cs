using Microsoft.Extensions.Logging;

namespace RapidCore.Mongo.Migration
{
    public class MigrationContext
    {
        public IConnectionProvider ConnectionProvider { get; set; }

        public ILogger Logger { get; set; }
    }
}