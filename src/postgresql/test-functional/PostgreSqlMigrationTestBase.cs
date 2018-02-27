using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using functionaltests.Migrations;
using Npgsql;
using RapidCore.Migration;
using RapidCore.PostgreSql.Internal;

namespace RapidCore.PostgreSql.FunctionalTests
{
    public abstract class PostgreSqlMigrationTestBase
    {
        private IDbConnection db;
        private bool isConnected = false;

        protected string ConnectionString { get; set; } = "User ID=postgres;Password=123456;Host=localhost;Port=5432;Database=postgres;";
        protected IDbConnection GetDb()
        {
            Connect();
            return db;
        }

        protected void Connect()
        {
            if (!isConnected)
            {
                this.db = new NpgsqlConnection(ConnectionString);
                isConnected = true;
            }
        }

        protected async Task DropMigrationInfoTable()
        {
            var db = GetDb();

            await db.ExecuteAsync($"DROP TABLE IF EXISTS {PostgreSqlConstants.CompletedStepsTableName}");
            await db.ExecuteAsync($"DROP TABLE IF EXISTS {PostgreSqlConstants.MigrationInfoTableName}");
        }

        protected async Task PrepareCounterTable(List<Counter> counters)
        {
            var db = GetDb();
            await db.ExecuteAsync("DROP TABLE IF EXISTS __Counter");
            await db.ExecuteAsync(@"CREATE TABLE __Counter (
                                    id serial not null
                                    constraint counter_pkey
                                    primary key,
                                    CounterValue int8
                                    );");

            foreach (var c in counters)
            {
                await this.InsertCounterEntity(c);
            }
        }

        protected async Task InsertCounterEntity(Counter entity)
        {
            var db = GetDb();
            string insertQuery = "INSERT INTO __Counter VALUES (@Id, @CounterValue)";
            await db.ExecuteAsync(insertQuery, new
            {
                Id = entity.Id,
                CounterValue = entity.CounterValue
            });
        }

        protected async Task<List<MigrationInfo>> GetAllMigrationInfo()
        {
            var db = GetDb();
            var migrationInfos = await db.QueryAsync<MigrationInfo>($"select * from {PostgreSqlConstants.MigrationInfoTableName}");
            return migrationInfos.AsList();
        }

    }
}