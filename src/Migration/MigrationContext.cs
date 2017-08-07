using Microsoft.Extensions.Logging;

namespace RapidCore.Mongo.Migration
{
    public class MigrationContext
    {
        public virtual IConnectionProvider ConnectionProvider { get; set; }

        public virtual ILogger Logger { get; set; }
        
        public virtual IContainerAdapter Container { get; set; }
        
        public virtual IMigrationEnvironment Environment { get; set; }
    }
}