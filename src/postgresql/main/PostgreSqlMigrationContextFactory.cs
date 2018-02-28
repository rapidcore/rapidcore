using System;
using System.Collections.Generic;
using System.Text;
using RapidCore.Migration;

namespace RapidCore.PostgreSql
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
