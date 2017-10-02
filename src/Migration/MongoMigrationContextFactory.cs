using RapidCore.Migration;

namespace RapidCore.Mongo.Migration
{
    public class MongoMigrationContextFactory : IMigrationContextFactory
    {
        private readonly ConnectionProvider connectionProvider;

        public MongoMigrationContextFactory(ConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public IMigrationContext GetContext()
        {
            return new MongoMigrationContext
            {
                ConnectionProvider = connectionProvider
            };
        }
    }
}