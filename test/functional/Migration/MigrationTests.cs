using System.Collections.Generic;
using System.Reflection;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RapidCore.Locking;
using RapidCore.Mongo.Migration;
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
            var services = new ServiceCollection();
            var connectionProvider = new ConnectionProvider();
            
            var runner = new MigrationRunner(
                new LoggerFactory().CreateLogger<MigrationRunner>(),
                new ServiceProviderContainerAdapter(services.BuildServiceProvider()), 
                new MigrationEnvironment("staging"), 
                connectionProvider,
                new MigrationManager(new List<Assembly> {typeof(MigrationTests).GetAssembly()}),
                A.Fake<IDistributedAppLockProvider>()
            );
            
            // TODO setup some state

            await runner.UpgradeAsync();
            
            // TODO make some checks
        }
    }
}