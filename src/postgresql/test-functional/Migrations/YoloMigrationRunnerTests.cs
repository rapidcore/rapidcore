using Dapper;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using RapidCore.Locking;
using RapidCore.PostgreSql.FunctionalTests.Migrations.TestMigrations;
using RapidCore.PostgreSql.Migration;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace RapidCore.PostgreSql.FunctionalTests.Migrations
{
    public class YoloMigrationRunnerTests : PostgreSqlMigrationTestBase
    {
        [Fact]
        public async void RunMigrations_Works()
        {
            await DropMigrationInfoTable();
            await PrepareCounterTable(new List<Counter> { new Counter { Id = 123, CounterValue = 12 } });

            var db = GetDb();

            var services = new ServiceCollection();

            var runner = new YoloMigrationRunner(
                services.BuildServiceProvider(),
                "staging",
                ConnectionString,
                A.Fake<IDistributedAppLockProvider>(),
                typeof(YoloMigrationRunnerTests).GetTypeInfo().Assembly
            );

            await runner.UpgradeAsync();

            // are all the migrations marked as completed?
            var migrationInfos = await GetAllMigrationInfo();

            Assert.Contains(migrationInfos, x => x.Name == nameof(Migration01) && x.MigrationCompleted);
            Assert.Contains(migrationInfos, x => x.Name == nameof(Migration02) && x.MigrationCompleted);

            // check the state of the db
            var counter999 = await db.QuerySingleAsync<Counter>("select * from __Counter where Id = 123");
            Assert.Equal("sample default value", counter999.Description);

            await DropCounterTable();
        }
    }
}
