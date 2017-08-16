using System.Collections.Generic;
using System.Reflection;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RapidCore.DependencyInjection;
using RapidCore.FunctionalTests.Migration.Implementation;
using RapidCore.FunctionalTests.Migration.TheMigrations;
using RapidCore.Locking;
using RapidCore.Migration;
using Xunit;

namespace RapidCore.FunctionalTests.Migration
{
    public class MigrationStackTests
    {
        [Fact]
        public async void RunMigration()
        {
            var db = new FuncMigrationDatabase();
            var services = new ServiceCollection();
            var contextFactory = A.Fake<IMigrationContextFactory>();
            var context = new FuncMigrationContext {Database = db};

            A.CallTo(() => contextFactory.GetContext()).Returns(context);

            var migrationManager = new FuncMigrationManager(new List<Assembly> {typeof(MigrationStackTests).GetTypeInfo().Assembly});
            
            var runner = new MigrationRunner(
                new LoggerFactory().CreateLogger<MigrationRunner>(),
                new ServiceProviderRapidContainerAdapter(services.BuildServiceProvider()), 
                new MigrationEnvironment("staging"), 
                migrationManager,
                A.Fake<IDistributedAppLockProvider>(),
                contextFactory
            );
            
            // setup some state
            var five = new FuncMigrationKewlEntity {Id = "five", Reference = "five"};
            db.UpsertKewl(five);
            var seven = new FuncMigrationKewlEntity {Id = "seven", Reference = "seven"};
            db.UpsertKewl(seven);
            
            // let's say that migration01 has already been completed
            await migrationManager.MarkAsCompleteAsync(new Migration01(), 123, context);

            await runner.UpgradeAsync();
            
            // are all the migrations marked as completed?
            var allDocs = db.AllMigrationInfos();
            Assert.Contains(allDocs, x => x.Name == nameof(Migration01) && x.MigrationCompleted);
            Assert.Contains(allDocs, x => x.Name == nameof(Migration02) && x.MigrationCompleted);
            Assert.Contains(allDocs, x => x.Name == nameof(Migration03) && x.MigrationCompleted);
            
            // check the state of the db
            var fiveUp = db.GetKewlById("five");
            Assert.Equal("Mucho five yay", fiveUp.Reference);
            
            var sevenUp = db.GetKewlById("seven");
            Assert.Equal("Mucho seven yay", sevenUp.Reference);
        }
    }
}