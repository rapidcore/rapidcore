using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using RapidCore.Migration;
using RapidCore.PostgreSql.Internal;

namespace Rapidcore.Postgresql
{
    public class PostgreSqlMigrationStorage : IMigrationStorage
    {
        public async Task MarkAsCompleteAsync(IMigrationContext context, IMigration migration, long milliseconds)
        {
            var info = await GetMigrationInfoAsync(context, migration.Name);
            if (info == default(MigrationInfo))
            {
                info = new MigrationInfo
                {
                    Name = migration.Name
                };
            }

            info.CompletedAtUtc = DateTime.UtcNow;
            info.MigrationCompleted = true;
            info.TotalMigrationTimeInMs = milliseconds;

            await UpsertMigrationInfoAsync(context, info);
        }

        private IDbConnection GetDb(IMigrationContext context)
        {
            return ((PostgreSqlMigrationContext) context).ConnectionProvider.Default();
        }

        public async Task<MigrationInfo> GetMigrationInfoAsync(IMigrationContext context, string migrationName)
        {
            var db = GetDb(context);
            var migrationInfo = await db.QuerySingleOrDefaultAsync<MigrationInfo>($"select * from {PostgreSqlConstants.MigrationInfoTableName} where Name = @MigrationName",
                new {
                    MigrationName = migrationName
                });
            return migrationInfo;
        }

        public async Task UpsertMigrationInfoAsync(IMigrationContext context, MigrationInfo info)
        {
            // TODO: steps completed
            // TODO: support update
            var db = GetDb(context);
            if (info.Id != null)
            {
                int id = Convert.ToInt32(info.Id);
                string updateQuery = $@"UPDATE {PostgreSqlConstants.MigrationInfoTableName}
                                       SET
                                        Name = @Name,
                                        MigrationCompleted = @MigrationCompleted,
                                        TotalMigrationTimeInMs = @TotalMigrationTimeInMs,
                                        CompletedAtUtc = @CompletedAtUtc
                                       WHERE id=@Id";
                await db.ExecuteAsync(updateQuery, new
                {
                    Id = id,
                    Name = info.Name,
                    MigrationCompleted = info.MigrationCompleted,
                    TotalMigrationTimeInMs = info.TotalMigrationTimeInMs,
                    CompletedAtUtc = info.CompletedAtUtc
                });
            }
            else
            {
                string insertQuery = $@"INSERT INTO {PostgreSqlConstants.MigrationInfoTableName}(
                                        Name,
                                        MigrationCompleted,
                                        TotalMigrationTimeInMs,
                                        CompletedAtUtc) 
                                   VALUES (
                                        @Name,
                                        @MigrationCompleted,
                                        @TotalMigrationTimeInMs,
                                        @CompletedAtUtc)";

                await db.ExecuteAsync(insertQuery, new
                {
                    Name = info.Name,
                    MigrationCompleted = info.MigrationCompleted,
                    TotalMigrationTimeInMs = info.TotalMigrationTimeInMs,
                    CompletedAtUtc = info.CompletedAtUtc
                });


            }
        }

        public async Task<bool> HasMigrationBeenFullyCompletedAsync(IMigrationContext context, string migrationName)
        {
            // this is the de facto entry point via the migration runner, so create the table if it doesn't exist
            // there might be a better place for this
            var db = GetDb(context);
            await db.ExecuteAsync($@"CREATE TABLE IF NOT EXISTS {PostgreSqlConstants.MigrationInfoTableName} (
                                    id serial not null
                                    constraint migrationinfo_pkey
                                    primary key,
                                    Name text,
                                    MigrationCompleted boolean,
                                    TotalMigrationTimeInMs int8,
                                    CompletedAtUtc timestamp
                                    );");

            var info = await GetMigrationInfoAsync(context, migrationName);

            return (info != default(MigrationInfo) && info.MigrationCompleted);
        }
    }
}
