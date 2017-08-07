using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.Locking;

namespace RapidCore.Mongo.Migration
{
    /// <summary>
    /// Runs migrations
    /// </summary>
    public class MigrationRunner
    {
        protected readonly ILogger<MigrationRunner> logger;
        protected readonly IContainerAdapter container;
        protected readonly IMigrationEnvironment environment;
        protected readonly IConnectionProvider connectionProvider;
        protected readonly IMigrationManager migrationManager;
        protected readonly IDistributedAppLockProvider appLocker;

        public MigrationRunner(
            ILogger<MigrationRunner> logger,
            IContainerAdapter container,
            IMigrationEnvironment environment,
            IConnectionProvider connectionProvider,
            IMigrationManager migrationManager,
            IDistributedAppLockProvider appLocker
        )
        {
            this.logger = logger;
            this.container = container;
            this.environment = environment;
            this.connectionProvider = connectionProvider;
            this.migrationManager = migrationManager;
            this.appLocker = appLocker;
        }

        protected virtual string GetLockName()
        {
            return "RapidCoreMigrations";
        }

        protected virtual MigrationContext GetContext()
        {
            return new MigrationContext
            {
                Logger = logger,
                ConnectionProvider = connectionProvider,
                Container =  container,
                Environment = environment
            };
        }

        /// <summary>
        /// Upgrade the enviroment
        /// </summary>
        public virtual async Task UpgradeAsync()
        {
            // 1. Ensure no one else runs migrations (i.e. lock)
            // 2. Find migrations
            // 3. Run each migration
            
            using (await appLocker.AcquireAsync(GetLockName(), TimeSpan.FromSeconds(30)))
            {
                logger.LogInformation($"Lock {GetLockName()} acquired");
                var sw = new Stopwatch();

                var context = GetContext();
                
                foreach (var migration in await migrationManager.FindMigrationsForUpgradeAsync())
                {
                    try
                    {
                        logger.LogInformation($"Attempt to run migration {migration.Name}");
                        sw.Restart();
                        await migration.UpgradeAsync(context);
                        sw.Stop();
                        logger.LogInformation($"Succeeded in running {migration.Name}. It took {sw.ElapsedMilliseconds} milliseconds.");
                        
                        await migrationManager.MarkAsCompleteAsync(migration, sw.ElapsedMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        sw.Stop();
                        logger.LogCritical(666, ex, $"Failed to run migration {migration.Name} after {sw.ElapsedMilliseconds} milliseconds.");
                        throw new MigrationException($"Failed to run migration {migration.Name}. See inner exception.", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Downgrade the enviroment
        /// </summary>
        public virtual async Task DowngradeAsync()
        {
            await Task.FromException(new NotImplementedException("Downgrade has not been implemented yet"));
        }
    }
}