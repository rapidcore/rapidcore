using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using RapidCore.Migration;

namespace rapidcore.postgresql
{
    public class PostgreSqlMigrationStorage : IMigrationStorage
    {
        public Task MarkAsCompleteAsync(IMigrationContext context, IMigration migration, long milliseconds)
        {
            var db = GetDb(context);
            return null;
        }

        private IDbConnection GetDb(IMigrationContext context)
        {
            return ((PostgreSqlMigrationContext) context).ConnectionProvider.Default();
        }

        public async Task<MigrationInfo> GetMigrationInfoAsync(IMigrationContext context, string migrationName)
        {
            var db = GetDb(context);
            var migrationInfo = await db.QuerySingleAsync<MigrationInfo>("select Name = @MigrationName", new { MigrationName = migrationName });
            return migrationInfo;
        }

        public Task UpsertMigrationInfoAsync(IMigrationContext context, MigrationInfo info)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasMigrationBeenFullyCompletedAsync(IMigrationContext context, string migrationName)
        {
            throw new NotImplementedException();
        }
    }
}
