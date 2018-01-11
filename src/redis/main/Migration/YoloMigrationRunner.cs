using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using RapidCore.DependencyInjection;
using RapidCore.Locking;
using RapidCore.Migration;
using StackExchange.Redis;

namespace RapidCore.Redis.Migration
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
        
        /// <summary>
        /// Bypasses dependency injection almost entirely to give an easy
        /// to get started with migration runner.
        /// 
        /// You must register <see cref="IConnectionMultiplexer"/> in <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The dependency injection container provided by dotnet Core</param>
        /// <param name="environmentName">The name of the environment you are running in</param>
        /// <param name="appLocker">The locker to use</param>
        /// <param name="assemblyWithMigrations">The assembly containing your migration classes</param>
        public YoloMigrationRunner(
            IServiceProvider services,
            string environmentName,
            IDistributedAppLockProvider appLocker,
            Assembly assemblyWithMigrations
        )
            : base(
                new LoggerFactory().CreateLogger<MigrationRunner>(),
                new ServiceProviderRapidContainerAdapter(services), 
                new MigrationEnvironment(environmentName), 
                appLocker,
                new RedisMigrationContextFactory(), 
                new ReflectionMigrationFinder(assemblyWithMigrations),
                new RedisMigrationStorage()
            )
        {
        }
    }
}