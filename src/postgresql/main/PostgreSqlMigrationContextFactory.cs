using System;
using System.Collections.Generic;
using System.Text;
using RapidCore.Migration;

namespace rapidcore.postgresql
{
    public class PostgreSqlMigrationContextFactory : IMigrationContextFactory
    {
        private readonly PostgreSqlConnectionProvider connectionProvider;

        public PostgreSqlMigrationContextFactory(PostgreSqlConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public IMigrationContext GetContext()
        {
            return new PostgreSqlMigrationContext
            {
                ConnectionProvider = connectionProvider
            };
        }

    }
}
