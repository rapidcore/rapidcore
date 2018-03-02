using RapidCore.Migration;

namespace RapidCore.PostgreSql.Migration
{
    public class PostgreSqlMigrationContextFactory : IMigrationContextFactory
    {
        private readonly PostgreSqlConnectionProvider connectionProvider;

        public PostgreSqlMigrationContextFactory(PostgreSqlConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public virtual IMigrationContext GetContext()
        {
            return new PostgreSqlMigrationContext
            {
                ConnectionProvider = connectionProvider
            };
        }

    }
}
