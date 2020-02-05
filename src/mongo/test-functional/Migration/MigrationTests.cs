using System.Collections.Generic;
using System.Reflection;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RapidCore.DependencyInjection;
using RapidCore.Locking;
using RapidCore.Migration;
using RapidCore.Mongo.FunctionalTests.Migration.TestMigration;
using RapidCore.Mongo.Migration;
using RapidCore.Mongo.Migration.Internal;
using RapidCore.Mongo.Testing;
using ServiceStack;
using Xunit;

namespace RapidCore.Mongo.FunctionalTests.Migration
{
    public class MigrationTests : MongoConnectedTestBase
    {
        [Fact]
        public async void RunMigration()
        {
            EnsureEmptyCollection<MigrationDocument>();
            EnsureEmptyCollection<KewlEntity>();
            
            var db = new MongoDbConnection(GetDb());
            var services = new ServiceCollection();
            
            var connectionProvider = new ConnectionProvider();
            connectionProvider.Add("x", db, true);
            
            var storage = new MongoMigrationStorage();

            var context = new MongoMigrationContext {ConnectionProvider = connectionProvider};
            
            var runner = new MigrationRunner(
                new LoggerFactory().CreateLogger<MigrationRunner>(),
                new ServiceProviderRapidContainerAdapter(services.BuildServiceProvider()), 
                new MigrationEnvironment("staging"), 
                A.Fake<IDistributedAppLockProvider>(),
                new MongoMigrationContextFactory(connectionProvider),
                new ReflectionMigrationFinder(new List<Assembly> {typeof(MigrationTests).GetAssembly()}),
                storage
            );
            
            // setup some state
            var five = new KewlEntity {Reference = 5};
            await db.InsertAsync<KewlEntity>(five);
            var seven = new KewlEntity {Reference = 7};
            await db.InsertAsync<KewlEntity>(seven);
            
            // let's say that migration01 has already been completed
            await storage.MarkAsCompleteAsync(context, new Migration01(), 123);

            await runner.UpgradeAsync();
            
            // are all the migrations marked as completed?
            var allDocs = GetAll<MigrationDocument>();
            Assert.Contains(allDocs, x => x.Name == nameof(Migration01) && x.MigrationCompleted);
            Assert.Contains(allDocs, x => x.Name == nameof(Migration02) && x.MigrationCompleted);
            Assert.Contains(allDocs, x => x.Name == nameof(Migration03) && x.MigrationCompleted);
            
            // check the state of the db
            var fiveUp = await db.FirstOrDefaultAsync<KewlEntityUpdated>(x => x.Id == five.Id);
            Assert.Equal("5", fiveUp.Reference);
            Assert.Equal("Ulla Henriksen", fiveUp.Mucho);
            
            var sevenUp = await db.FirstOrDefaultAsync<KewlEntityUpdated>(x => x.Id == seven.Id);
            Assert.Equal("7", sevenUp.Reference);
            Assert.Equal("Bubbly", sevenUp.Mucho);
        }
    }
}