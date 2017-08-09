using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using RapidCore.Locking;
using RapidCore.Mongo.FunctionalTests.Migration.TestMigration;
using RapidCore.Mongo.Migration;
using RapidCore.Mongo.Migration.Internal;
using RapidCore.Mongo.Testing;
using ServiceStack;
using Xunit;

namespace RapidCore.Mongo.FunctionalTests.Migration
{
    public class YoloMigrationRunnerTests : MongoConnectedTestBase
    {
        [Fact]
        public async void RunMigration()
        {
            EnsureEmptyCollection(MigrationDocument.CollectionName);
            EnsureEmptyCollection(KewlEntity.Collection);
            
            var db = new MongoDbConnection(GetDb());
            
            var services = new ServiceCollection();

            var runner = new YoloMigrationRunner(
                services.BuildServiceProvider(),
                "staging",
                ConnectionString,
                GetDbName(),
                A.Fake<IDistributedAppLockProvider>(),
                typeof(YoloMigrationRunnerTests).GetAssembly()
            );
            
            var migrationManager = new MigrationManager(typeof(YoloMigrationRunnerTests).GetAssembly());
            
            var connectionProvider = new ConnectionProvider();
            connectionProvider.Add("x", db, true);
            
            // setup some state
            var five = new KewlEntity {Reference = 5};
            await db.InsertAsync<KewlEntity>(KewlEntity.Collection, five);
            var seven = new KewlEntity {Reference = 7};
            await db.InsertAsync<KewlEntity>(KewlEntity.Collection, seven);
            
            // let's say that migration01 has already been completed
            await migrationManager.MarkAsCompleteAsync(new Migration01(), 123, new MigrationContext { ConnectionProvider = connectionProvider });

            await runner.UpgradeAsync();
            
            // are all the migrations marked as completed?
            var allDocs = GetAll<MigrationDocument>(MigrationDocument.CollectionName);
            Assert.Contains(allDocs, x => x.Name == nameof(Migration01) && x.MigrationCompleted);
            Assert.Contains(allDocs, x => x.Name == nameof(Migration02) && x.MigrationCompleted);
            Assert.Contains(allDocs, x => x.Name == nameof(Migration03) && x.MigrationCompleted);
            
            // check the state of the db
            var fiveUp = await db.FirstOrDefaultAsync<KewlEntityUpdated>(KewlEntity.Collection, x => x.Id == five.Id);
            Assert.Equal("5", fiveUp.Reference);
            Assert.Equal("Ulla Henriksen", fiveUp.Mucho);
            
            var sevenUp = await db.FirstOrDefaultAsync<KewlEntityUpdated>(KewlEntity.Collection, x => x.Id == seven.Id);
            Assert.Equal("7", sevenUp.Reference);
            Assert.Equal("Bubbly", sevenUp.Mucho);
        }
    }
}