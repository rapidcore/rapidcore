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
        private readonly IDistributedAppLockProvider appLocker;
        private readonly IMigrationContextFactory contextFactory;
        private readonly IMigrationFinder finder;
        private readonly IMigrationStorage storage;
        private readonly IMigrationContext context;
        private readonly IDistributedAppLock appLock;
        private readonly IMigration migration1;
        private readonly IMigration migration2;

        public UpgradeAsyncTests()
        {
            logger = A.Fake<ILogger<MigrationRunner>>();
            container = A.Fake<IRapidContainerAdapter>();
            environment = A.Fake<IMigrationEnvironment>();
            appLocker = A.Fake<IDistributedAppLockProvider>();
            contextFactory = A.Fake<IMigrationContextFactory>();
            finder = A.Fake<IMigrationFinder>();
            storage = A.Fake<IMigrationStorage>();
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
                appLocker,
                contextFactory,
                finder,
                storage
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
        public async void Upgrade_GetMigrationsFromFinder()
        {
            await runner.UpgradeAsync();

            A.CallTo(() => finder.FindMigrationsForUpgradeAsync(context)).MustHaveHappened();
        }
        
        [Fact]
        public async void Upgrade_CallUpgradeOnAllMigrations()
        {
            A.CallTo(() => finder.FindMigrationsForUpgradeAsync(A<IMigrationContext>._)).Returns(Task.FromResult<IList<IMigration>>(new List<IMigration> {migration1, migration2}));
            
            await runner.UpgradeAsync();

            A.CallTo(() => migration1.UpgradeAsync(context)).MustHaveHappened();
            A.CallTo(() => migration2.UpgradeAsync(context)).MustHaveHappened();
        }
        
        [Fact]
        public async void Upgrade_MarkMigrationAsComplete()
        {
            A.CallTo(() => finder.FindMigrationsForUpgradeAsync(A<IMigrationContext>._)).Returns(Task.FromResult<IList<IMigration>>(new List<IMigration> {migration1}));
            
            await runner.UpgradeAsync();

            A.CallTo(() => storage.MarkAsCompleteAsync(context, migration1, A<long>._)).MustHaveHappened();
        }
        
        [Fact]
        public async void Upgrade_ifError_doNotMarkAsComplete_wrapThrownException()
        {
            var innerException = new Exception("DIE!");
            A.CallTo(() => finder.FindMigrationsForUpgradeAsync(A<IMigrationContext>._)).Returns(Task.FromResult<IList<IMigration>>(new List<IMigration> {migration1}));
            A.CallTo(() => migration1.UpgradeAsync(A<IMigrationContext>._)).ThrowsAsync(innerException);

            var actual = await Record.ExceptionAsync(async () => await runner.UpgradeAsync());
            
            Assert.NotNull(actual);
            Assert.Same(innerException, actual.InnerException);
            A.CallTo(() => storage.MarkAsCompleteAsync(A<IMigrationContext>._, A<IMigration>._, A<long>._)).MustNotHaveHappened();
        }
    }
}