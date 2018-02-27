using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using functionaltests.Migrations;
using functionaltests.Migrations.TestMigrations;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rapidcore.Postgresql;
using RapidCore.DependencyInjection;
using RapidCore.Locking;
using RapidCore.Migration;
using RapidCore.PostgreSql;
using ServiceStack;
using Xunit;

namespace RapidCore.PostgreSql.FunctionalTests
{
    public class MigrationTests : PostgreSqlMigrationTestBase
    {
        [Fact]
        public async Task RunMigration()
        {
            await PrepareMigrationInfoTable();
            await PrepareCounterTable(new List<Counter>{new Counter{Id = 999, CounterValue = 12}});

            var db = GetDb();

            var services = new ServiceCollection();

            var provider = new PostgreSqlConnectionProvider();
            provider.Add("yolo", db, true);
            
            var runner = new MigrationRunner(
                new LoggerFactory().CreateLogger<MigrationRunner>(),
                new ServiceProviderRapidContainerAdapter(services.BuildServiceProvider()),
                new MigrationEnvironment("staging"),
                A.Fake<IDistributedAppLockProvider>(),
                new PostgreSqlMigrationContextFactory(provider),
                new ReflectionMigrationFinder(new List<Assembly> { typeof(MigrationTests).GetAssembly() }),
                new PostgreSqlMigrationStorage()
                );

            await runner.UpgradeAsync();

            // assert all migrations are marked as complete
            var migrationInfos = await GetAllMigrationInfo();
            Assert.Contains(migrationInfos, x => x.Name == nameof(Migration01) && x.MigrationCompleted);
            Assert.Contains(migrationInfos, x => x.Name == nameof(Migration02) && x.MigrationCompleted);

            // check the state of the db
            var counter999 = await db.QuerySingleAsync<Counter>("select * from __Counter where Id = 999");
            Assert.Equal("sample default value", counter999.Description);

        }
    }
}
