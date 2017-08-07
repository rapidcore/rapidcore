using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using RapidCore.Locking;
using RapidCore.Mongo.Migration;
using Xunit;

namespace RapidCore.Mongo.UnitTests.Migration
{
    public class MigrationRunnerUpgradeTests
    {
        private readonly MigrationRunner runner;
        private readonly IContainerAdapter container;
        private readonly IMigrationEnvironment environment;
        private readonly IConnectionProvider connectionProvider;
        private readonly IMigrationManager migrationManager;
        private readonly IDistributedAppLockProvider appLocker;
        private readonly IDistributedAppLock appLock;
        private readonly IMigration migration1;
        private readonly IMigration migration2;

        public MigrationRunnerUpgradeTests()
        {
            container = A.Fake<IContainerAdapter>();
            environment = A.Fake<IMigrationEnvironment>();
            connectionProvider = A.Fake<IConnectionProvider>();
            migrationManager = A.Fake<IMigrationManager>();
            appLocker = A.Fake<IDistributedAppLockProvider>();
            appLock = A.Fake<IDistributedAppLock>();
            migration1 = A.Fake<IMigration>();
            migration2 = A.Fake<IMigration>();

            A.CallTo(() => appLocker.AcquireAsync("RapidCoreMigrations", A<TimeSpan>._)).Returns(Task.FromResult(appLock));
            
            runner = new MigrationRunner(
                A.Fake<ILogger<MigrationRunner>>(),
                container,
                environment,
                connectionProvider,
                migrationManager,
                appLocker
            );
        }

        [Fact]
        public async void Upgrade_Locks()
        {
            await runner.UpgradeAsync();

            A.CallTo(() => appLocker.AcquireAsync("RapidCoreMigrations", A<TimeSpan>.That.Matches(x => x == TimeSpan.FromSeconds(30)))).MustHaveHappened();
        }
        
        [Fact]
        public async void Upgrade_GetMigrationsFromManager()
        {
            await runner.UpgradeAsync();

            A.CallTo(() => migrationManager.FindMigrationsForUpgradeAsync()).MustHaveHappened();
        }
        
        [Fact]
        public async void Upgrade_CallUpgradeOnAllMigrations()
        {
            A.CallTo(() => migrationManager.FindMigrationsForUpgradeAsync()).Returns(Task.FromResult<IList<IMigration>>(new List<IMigration> {migration1, migration2}));
            
            await runner.UpgradeAsync();

            A.CallTo(() => migration1.UpgradeAsync()).MustHaveHappened();
            A.CallTo(() => migration2.UpgradeAsync()).MustHaveHappened();
        }
        
        [Fact]
        public async void Upgrade_MarkMigrationAsComplete()
        {
            A.CallTo(() => migrationManager.FindMigrationsForUpgradeAsync()).Returns(Task.FromResult<IList<IMigration>>(new List<IMigration> {migration1}));
            
            await runner.UpgradeAsync();

            A.CallTo(() => migrationManager.MarkAsCompleteAsync(migration1, A<long>._)).MustHaveHappened();
        }
        
        [Fact]
        public async void Upgrade_ifError_doNotMarkAsComplete_wrapThrownException()
        {
            var innerException = new Exception("DIE!");
            A.CallTo(() => migrationManager.FindMigrationsForUpgradeAsync()).Returns(Task.FromResult<IList<IMigration>>(new List<IMigration> {migration1}));
            A.CallTo(() => migration1.UpgradeAsync()).ThrowsAsync(innerException);

            var actual = await Record.ExceptionAsync(async () => await runner.UpgradeAsync());
            
            Assert.NotNull(actual);
            Assert.Same(innerException, actual.InnerException);
            A.CallTo(() => migrationManager.MarkAsCompleteAsync(A<IMigration>._, A<long>._)).MustNotHaveHappened();
        }
    }
}