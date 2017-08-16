using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RapidCore.DependencyInjection;
using RapidCore.Locking;
using RapidCore.Migration;

namespace RapidCore.Mongo.Migration
{
    /// <summary>
    /// Simplified <see cref="MigrationRunner"/>, which is meant as a
    /// "welcome to migrations"-runner, as it provides a lower barrier
    /// of entry.
    /// 
    /// If you are using dependency injection, we strongly recommend
    /// you use <see cref="MigrationRunner"/> directly.
    /// </summary>
    public class YoloMigrationRunner : MigrationRunner
    {
        public YoloMigrationRunner(
            IServiceProvider services,
            string environmentName,
            string mongoConnectionString,
            string mongoDbName,
            IDistributedAppLockProvider appLocker,
            Assembly assemblyWithMigrations
        )
        : base(
            new LoggerFactory().CreateLogger<MigrationRunner>(),
            new ServiceProviderRapidContainerAdapter(services), 
            new MigrationEnvironment(environmentName), 
            appLocker,
            new MongoMigrationContextFactory(ConnectionProviderFromString(mongoConnectionString, mongoDbName)), 
            new ReflectionMigrationFinder(assemblyWithMigrations),
            new MongoMigrationStorage()
        )
        {
        }

        private static ConnectionProvider ConnectionProviderFromString(string mongoConnectionString, string mongoDbName)
        {
            var provider = new ConnectionProvider();
            var client = new MongoClient(mongoConnectionString);
            var db = client.GetDatabase(mongoDbName);
            var conn = new MongoDbConnection(db);
            
            provider.Add("yolo", conn, true);

            return provider;
        }
    }
}