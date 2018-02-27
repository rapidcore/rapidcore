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
            var db = GetDb(context);
            if (info.Id != null)
            {
                await HandleUpdate(info, db);
            }
            else
            {
                await HandleInsert(info, db);
            }
        }

        private static async Task HandleInsert(MigrationInfo info, IDbConnection db)
        {
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
                                        @MigrationInfoId)";

                await db.ExecuteAsync(completedStepInsertQuery, new
                {
                    StepName = stepName,
                    MigrationInfoId = migrationInfoId
                });

            }
        }

        private static async Task HandleUpdate(MigrationInfo info, IDbConnection db)
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

            foreach (var stepName in info.StepsCompleted)
            {
                string completedStepInsertQuery = $@"UPDATE {PostgreSqlConstants.CompletedStepsTableName}
                                                    SET
                                                        StepName = @StepName,
                                                        MigrationInfoId = @MigrationInfoId";

                await db.ExecuteAsync(completedStepInsertQuery, new
                {
                    StepName = stepName,
                    MigrationInfoId = info.Id
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

            await db.ExecuteAsync($@"CREATE TABLE IF NOT EXISTS {PostgreSqlConstants.CompletedStepsTableName} (
                                        StepName text,
                                        MigrationInfoId integer references {PostgreSqlConstants.MigrationInfoTableName} (id)
                                    );");



            var info = await GetMigrationInfoAsync(context, migrationName);

            return (info != default(MigrationInfo) && info.MigrationCompleted);
        }
    }
}
