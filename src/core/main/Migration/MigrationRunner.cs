using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RapidCore.DependencyInjection;
using RapidCore.Locking;

namespace RapidCore.Migration
{
    /// <summary>
    /// Runs migrations
    /// </summary>
    public class MigrationRunner
    {
        protected readonly ILogger<MigrationRunner> logger;
        protected readonly IRapidContainerAdapter container;
        protected readonly IMigrationEnvironment environment;
        protected readonly IDistributedAppLockProvider appLocker;
        protected readonly IMigrationContextFactory contextFactory;
        protected readonly IMigrationFinder finder;
        protected readonly IMigrationStorage storage;

        public MigrationRunner(
            ILogger<MigrationRunner> logger,
            IRapidContainerAdapter container,
            IMigrationEnvironment environment,
            IDistributedAppLockProvider appLocker,
            IMigrationContextFactory contextFactory,
            IMigrationFinder finder,
            IMigrationStorage storage
        )
        {
            this.logger = logger;
            this.container = container;
            this.environment = environment;
            this.appLocker = appLocker;
            this.contextFactory = contextFactory;
            this.finder = finder;
            this.storage = storage;
        }

        protected virtual string GetLockName()
        {
            return "RapidCoreMigrations";
        }

        protected virtual IMigrationContext GetContext()
        {
            var ctx = contextFactory.GetContext();
            ctx.Logger = logger;
            ctx.Environment = environment;
            ctx.Container = container;
            ctx.Storage = storage;

            return ctx;
        }

        /// <summary>
        /// Upgrade the enviroment
        /// </summary>
        public virtual async Task UpgradeAsync()
        {
            // 1. Ensure no one else runs migrations (i.e. lock)
            // 2. Find migrations
            // 3. Run each migration
            // 4. Mark successfully run migrations as completed
            
            using (await appLocker.AcquireAsync(GetLockName(), TimeSpan.FromSeconds(30), TimeSpan.MaxValue))
            {
                logger.LogInformation($"Lock {GetLockName()} acquired");
                var sw = new Stopwatch();

                var context = GetContext();
                
                foreach (var migration in await finder.FindMigrationsForUpgradeAsync(context))
                {
                    try
                    {
                        logger.LogInformation($"Attempt to run migration {migration.Name}");
                        sw.Restart();
                        await migration.UpgradeAsync(context);
                        sw.Stop();
                        logger.LogInformation($"Succeeded in running {migration.Name}. It took {sw.ElapsedMilliseconds} milliseconds.");
                        
                        await storage.MarkAsCompleteAsync(context, migration, sw.ElapsedMilliseconds);
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
        public virtual Task DowngradeAsync()
        {
            return Task.FromException(new NotImplementedException("Downgrade has not been implemented yet. Provide your input in https://github.com/rapidcore/rapidcore/issues/30"));
        }
    }
}