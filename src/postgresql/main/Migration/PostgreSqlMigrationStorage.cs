using Dapper;
using RapidCore.Migration;
using RapidCore.PostgreSql.Internal;
using System;
using System.Data;
using System.Threading.Tasks;
using RapidCore.PostgreSql.Migration.Internal;

namespace RapidCore.PostgreSql.Migration
{
    public class PostgreSqlMigrationStorage : IMigrationStorage
    {
        public virtual async Task MarkAsCompleteAsync(IMigrationContext context, IMigration migration, long milliseconds)
        {
            var db = await EnsureDbHasBeenInitialized(context);
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

        private async Task<IDbConnection> EnsureDbHasBeenInitialized(IMigrationContext context)
        {
            var db = ((PostgreSqlMigrationContext) context).ConnectionProvider.Default();
            await PostgreSqlSchemaCreator.CreateSchemaIfNotExists(context);
            return db;

        }

        public virtual async Task<MigrationInfo> GetMigrationInfoAsync(IMigrationContext context, string migrationName)
        {
            var db = await EnsureDbHasBeenInitialized(context);
            var migrationInfo = await db.QuerySingleOrDefaultAsync<MigrationInfo>($"select * from {PostgreSqlConstants.MigrationInfoTableName} where Name = @MigrationName",
                new {
                    MigrationName = migrationName
                });
            return migrationInfo;
        }

        public virtual async Task UpsertMigrationInfoAsync(IMigrationContext context, MigrationInfo info)
        {
            var db = await EnsureDbHasBeenInitialized(context);

            long migrationInfoId = 0;
            string insertQuery = $@"INSERT INTO {PostgreSqlConstants.MigrationInfoTableName}(
                                        Name,
                                        MigrationCompleted,
                                        TotalMigrationTimeInMs,
                                        CompletedAtUtc) 
                                   VALUES (
                                        @Name,
                                        @MigrationCompleted,
                                        @TotalMigrationTimeInMs,
                                        @CompletedAtUtc)
                                   ON CONFLICT (name)
                                   DO UPDATE SET
                                        Name = @Name,
                                        MigrationCompleted = @MigrationCompleted,
                                        TotalMigrationTimeInMs = @TotalMigrationTimeInMs,
                                        CompletedAtUtc = @CompletedAtUtc
                                   RETURNING id";

            using (var reader = await db.ExecuteReaderAsync(insertQuery, new
            {
                Name = info.Name,
                MigrationCompleted = info.MigrationCompleted,
                TotalMigrationTimeInMs = info.TotalMigrationTimeInMs,
                CompletedAtUtc = info.CompletedAtUtc
            }))
            {
                while (reader.Read())
                {
                    migrationInfoId = reader.GetInt64(0);
                }
            }
            foreach (var stepName in info.StepsCompleted)
            {
                string completedStepInsertQuery = $@"INSERT INTO {PostgreSqlConstants.CompletedStepsTableName}(
                                        StepName,
                                        MigrationInfoId)
                                   VALUES (
                                        @StepName,
                                        @MigrationInfoId)
                                   ON CONFLICT (StepName, MigrationInfoId)
                                   DO NOTHING
                                    ";

                await db.ExecuteAsync(completedStepInsertQuery, new
                {
                    StepName = stepName,
                    MigrationInfoId = migrationInfoId
                });

            }
        }

        public virtual async Task<bool> HasMigrationBeenFullyCompletedAsync(IMigrationContext context, string migrationName)
        {
            var db = await EnsureDbHasBeenInitialized(context);
            var info = await GetMigrationInfoAsync(context, migrationName);

            return (info != default(MigrationInfo) && info.MigrationCompleted);
        }
    }
}
