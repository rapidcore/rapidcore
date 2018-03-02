using Dapper;
using RapidCore.Migration;
using RapidCore.PostgreSql.Internal;
using System.Threading.Tasks;

namespace RapidCore.PostgreSql.Migration.Internal
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
                                        PRIMARY KEY (StepName, MigrationInfoId)
                                    );");
        }

    }
}
