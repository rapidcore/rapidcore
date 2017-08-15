using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using RapidCore.DependencyInjection;
using RapidCore.Locking;
using RapidCore.Migration;
using Xunit;

namespace RapidCore.UnitTests.Migration.MigrationRunnerTests
{
    public class UpgradeAsyncTests
    {
        private readonly MigrationRunner runner;
        private readonly ILogger<MigrationRunner> logger;
        private readonly IRapidContainerAdapter container;
        private readonly IMigrationEnvironment environment;
        private readonly IMigrationManager migrationManager;
        private readonly IDistributedAppLockProvider appLocker;
        private readonly IMigrationContextFactory contextFactory;
        private readonly IMigrationContext context;
        private readonly IDistributedAppLock appLock;
        private readonly IMigration migration1;
        private readonly IMigration migration2;

        public UpgradeAsyncTests()
        {
            logger = A.Fake<ILogger<MigrationRunner>>();
            container = A.Fake<IRapidContainerAdapter>();
            environment = A.Fake<IMigrationEnvironment>();
            migrationManager = A.Fake<IMigrationManager>();
            appLocker = A.Fake<IDistributedAppLockProvider>();
            contextFactory = A.Fake<IMigrationContextFactory>();
            context = A.Fake<IMigrationContext>();
            appLock = A.Fake<IDistributedAppLock>();
            migration1 = A.Fake<IMigration>();
            migration2 = A.Fake<IMigration>();

            A.CallTo(() => appLocker.AcquireAsync("RapidCoreMigrations", A<TimeSpan>._)).Returns(Task.FromResult(appLock));
            A.CallTo(() => contextFactory.GetContext()).Returns(context);
            
            runner = new MigrationRunner(
                logger,
                container,
                environment,
                migrationManager,
                appLocker,
                contextFactory
            );
        }

        [Fact]
        public async void Upgrade_Locks()
        {
            await runner.UpgradeAsync();

            A.CallTo(() => appLocker.AcquireAsync("RapidCoreMigrations", A<TimeSpan>.That.Matches(x => x == TimeSpan.FromSeconds(30)))).MustHaveHappened();
        }

        [Fact]
        public async void Upgrade_SetsLoggerEtcOnContext()
        {
            await runner.UpgradeAsync();

            A.CallToSet(() => context.Container).To(container).MustHaveHappened();
            A.CallToSet(() => context.Environment).To(environment).MustHaveHappened();
            A.CallToSet(() => context.Logger).To(logger).MustHaveHappened();
        }
        
        [Fact]
        public async void Upgrade_GetMigrationsFromManager()
        {
            await runner.UpgradeAsync();

            A.CallTo(() => migrationManager.FindMigrationsForUpgradeAsync(context)).MustHaveHappened();
        }
        
        [Fact]
        public async void Upgrade_CallUpgradeOnAllMigrations()
        {
            A.CallTo(() => migrationManager.FindMigrationsForUpgradeAsync(A<IMigrationContext>._)).Returns(Task.FromResult<IList<IMigration>>(new List<IMigration> {migration1, migration2}));
            
            await runner.UpgradeAsync();

            A.CallTo(() => migration1.UpgradeAsync(context)).MustHaveHappened();
            A.CallTo(() => migration2.UpgradeAsync(context)).MustHaveHappened();
        }
        
        [Fact]
        public async void Upgrade_MarkMigrationAsComplete()
        {
            A.CallTo(() => migrationManager.FindMigrationsForUpgradeAsync(A<IMigrationContext>._)).Returns(Task.FromResult<IList<IMigration>>(new List<IMigration> {migration1}));
            
            await runner.UpgradeAsync();

            A.CallTo(() => migrationManager.MarkAsCompleteAsync(migration1, A<long>._, context)).MustHaveHappened();
        }
        
        [Fact]
        public async void Upgrade_ifError_doNotMarkAsComplete_wrapThrownException()
        {
            var innerException = new Exception("DIE!");
            A.CallTo(() => migrationManager.FindMigrationsForUpgradeAsync(A<IMigrationContext>._)).Returns(Task.FromResult<IList<IMigration>>(new List<IMigration> {migration1}));
            A.CallTo(() => migration1.UpgradeAsync(A<IMigrationContext>._)).ThrowsAsync(innerException);

            var actual = await Record.ExceptionAsync(async () => await runner.UpgradeAsync());
            
            Assert.NotNull(actual);
            Assert.Same(innerException, actual.InnerException);
            A.CallTo(() => migrationManager.MarkAsCompleteAsync(A<IMigration>._, A<long>._, A<IMigrationContext>._)).MustNotHaveHappened();
        }
    }
}