using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RapidCore.DependencyInjection;
using RapidCore.Locking;
using RapidCore.Migration;
using RapidCore.Redis.FunctionalTest.Migration.TestMigration;
using RapidCore.Redis.Migration;
using StackExchange.Redis;
using Xunit;

namespace RapidCore.Redis.FunctionalTest.Migration
{
    public class MigrationTests
    {
        private readonly IConnectionMultiplexer redisMuxer;
        private readonly string hostname;

        public MigrationTests()
        {
            hostname = "127.0.0.1:6379";
            redisMuxer = ConnectionMultiplexer.Connect($"{hostname},allowAdmin=true");
        }

        private async Task EnsureEmptyDb()
        {
            await redisMuxer.GetServer(hostname).FlushDatabaseAsync();
        }
        
        [Fact]
        public async void RunMigration()
        {
            await EnsureEmptyDb();

            var db = redisMuxer.GetDatabase();
            var services = new ServiceCollection();
            services.AddSingleton<IConnectionMultiplexer>(redisMuxer);
            
            var storage = new RedisMigrationStorage();

            var context = new RedisMigrationContext { Container = new ServiceProviderRapidContainerAdapter(services.BuildServiceProvider()) };
            
            var runner = new MigrationRunner(
                new LoggerFactory().CreateLogger<MigrationRunner>(),
                new ServiceProviderRapidContainerAdapter(services.BuildServiceProvider()), 
                new MigrationEnvironment("staging"), 
                A.Fake<IDistributedAppLockProvider>(),
                new RedisMigrationContextFactory(),
                new ReflectionMigrationFinder(new List<Assembly> {typeof(MigrationTests).GetTypeInfo().Assembly}),
                storage
            );
            
            // setup some state
            await db.StringSetAsync("five", "5");
            await db.StringSetAsync("seven", "7");
            
            // let's say that migration01 has already been completed
            await storage.MarkAsCompleteAsync(context, new Migration01(), 123);

            await runner.UpgradeAsync();
            
            // are all the migrations marked as completed?
            var migrationInfos = await GetAllMigrationInfos();
            Assert.Equal(3, migrationInfos.Count);
            Assert.Contains(migrationInfos, x => x.Name == nameof(Migration01) && x.MigrationCompleted);
            Assert.Contains(migrationInfos, x => x.Name == nameof(Migration02) && x.MigrationCompleted);
            Assert.Contains(migrationInfos, x => x.Name == nameof(Migration03) && x.MigrationCompleted);
            
            // check the state of the db
            Assert.Equal("5 up", await db.StringGetAsync("five"));
            Assert.Equal("7 up", await db.StringGetAsync("seven"));
            Assert.Equal("OMG OMG OMG", await db.StringGetAsync("thirteen"));
        }

        private async Task<List<MigrationInfo>> GetAllMigrationInfos()
        {
            var infos = new List<MigrationInfo>();
            
            foreach (var key in redisMuxer.GetServer(hostname).Keys(pattern: $"{RedisMigrationStorage.KeyPrefix}*"))
            {
                var value = await redisMuxer.GetDatabase().StringGetAsync(key);
                infos.Add(JsonConvert.DeserializeObject<MigrationInfo>(value.ToString()));
            }

            return infos;
        }
    }
}