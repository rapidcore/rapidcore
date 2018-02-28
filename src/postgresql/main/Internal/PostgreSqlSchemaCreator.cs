using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using RapidCore.PostgreSql;
using RapidCore.Migration;

namespace RapidCore.PostgreSql.Internal
{
    public static class PostgreSqlSchemaCreator
    {
        public static async Task CreateSchemaIfNotExists(IMigrationContext context)
        {
            var db = ((PostgreSqlMigrationContext)context).ConnectionProvider.Default(); ;
            await db.ExecuteAsync($@"CREATE TABLE IF NOT EXISTS {PostgreSqlConstants.MigrationInfoTableName} (
                                    id serial not null
                                    constraint migrationinfo_pkey
                                    primary key,
                                    Name varchar(255) unique,
                                    MigrationCompleted boolean,
                                    TotalMigrationTimeInMs int8,
                                    CompletedAtUtc timestamp
                                    );");

            await db.ExecuteAsync($@"CREATE TABLE IF NOT EXISTS {PostgreSqlConstants.CompletedStepsTableName} (
                                        StepName varchar(255),
                                        MigrationInfoId integer references {
                    PostgreSqlConstants.MigrationInfoTableName
                } (id),
                                        unique (StepName, MigrationInfoId)
                                    );");
        }

    }
}
