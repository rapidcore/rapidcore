using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using RapidCore.Migration;
using RapidCore.Migration.Internal;
using Xunit;

namespace RapidCore.UnitTests.Migration
{
    public class MigrationBaseTest
    {
        private readonly ImplMigrationBase migration;
        private readonly IMigrationContext context;
        private readonly MigrationInfo info;

        public MigrationBaseTest()
        {
            context = A.Fake<IMigrationContext>();
            info = A.Fake<MigrationInfo>();

            migration = new ImplMigrationBase
            {
                ConfigureUpgradeAction = A.Fake<Action<IMigrationBuilder>>(),
                ConfigureDowngradeAction = A.Fake<Action<IMigrationBuilder>>(),
                GetMigrationInfoFunc = A.Fake<Func<MigrationInfo>>(),
                UpsertMigrationInfoAction = A.Fake<Action<MigrationInfo>>()
            };
        }

        #region UpgradeAsync
        [Fact]
        public async void UpgradeAsync_callsConfigureWithBuilder()
        {
            await migration.UpgradeAsync(context);

            A.CallTo(() => migration.ConfigureUpgradeAction(A<IMigrationBuilder>._)).MustHaveHappened();
        }
        
        [Fact]
        public async void UpgradeAsync_setsTheMigrationNameOnTheInfo()
        {
            A.CallTo(() => migration.GetMigrationInfoFunc.Invoke()).Returns(info);
            
            await migration.UpgradeAsync(context);

            A.CallToSet(() => info.Name).To(nameof(ImplMigrationBase)).MustHaveHappened();
        }
        
        [Fact]
        public async void UpgradeAsync_applyAndSaveEachStep_inSequence()
        {
            var step1Action = A.Fake<Action>();
            var step2Action = A.Fake<Action>();
            
            A.CallTo(() => migration.ConfigureUpgradeAction.Invoke(A<IMigrationBuilder>._)).Invokes((IMigrationBuilder builder) =>
            {
                builder.Step("one", step1Action);
                builder.Step("two", step2Action);
            });
            
            A.CallTo(() => migration.GetMigrationInfoFunc.Invoke()).Returns(info);
            
            await migration.UpgradeAsync(context);

            A.CallTo(() => step1Action.Invoke()).MustHaveHappened()
                .Then(A.CallTo(() => info.AddCompletedStep("one")).MustHaveHappened())
                .Then(A.CallTo(() => migration.UpsertMigrationInfoAction(info)).MustHaveHappened())
                .Then(A.CallTo(() => step2Action.Invoke()).MustHaveHappened())
                .Then(A.CallTo(() => info.AddCompletedStep("two")).MustHaveHappened())
                .Then(A.CallTo(() => migration.UpsertMigrationInfoAction(info)).MustHaveHappened());
        }
        #endregion

        #region GetPendingStepsAsync
        [Fact]
        public async void GetPendingStepsAsync_ifNoMigrationInfo_returnsAllSteps_and_newInfo()
        {
            var expectedAllSteps = new List<string> {"one", "two"};
            var builder = A.Fake<MigrationBuilder>();
            A.CallTo(() => builder.GetAllStepNames()).Returns(expectedAllSteps);
            
            A.CallTo(() => migration.GetMigrationInfoFunc.Invoke()).Returns(null);
            
            (var actualSteps, var actualInfo) = await migration.GetPendingStepsAsync(builder); 

            Assert.Same(expectedAllSteps, actualSteps);
            Assert.NotNull(actualInfo);
        }
        
        [Fact]
        public async void GetPendingStepsAsync_returnsFilteredSteps_andExistingInfo()
        {
            var allSteps = new List<string> {"one", "two", "three"};
            var builder = A.Fake<MigrationBuilder>();
            A.CallTo(() => builder.GetAllStepNames()).Returns(allSteps);
            
            A.CallTo(() => migration.GetMigrationInfoFunc.Invoke()).Returns(info);
            A.CallTo(() => info.StepsCompleted).Returns(new List<string> {"one", "two"});
            
            (var actualSteps, var actualInfo) = await migration.GetPendingStepsAsync(builder); 

            Assert.Equal(new List<string> {"three"}, actualSteps);
            Assert.Same(info, actualInfo);
        }
        #endregion

        #region Name
        [Fact]
        public void Name_returnsNameOfTheClass()
        {
            Assert.Equal("ImplMigrationBase", migration.Name);
        }
        #endregion
        
        #region Implementation
        private class ImplMigrationBase : MigrationBase
        {
            public Action<IMigrationBuilder> ConfigureUpgradeAction { get; set; }
            public Action<IMigrationBuilder> ConfigureDowngradeAction { get; set; }
            public Func<MigrationInfo> GetMigrationInfoFunc { get; set; }
            public Action<MigrationInfo> UpsertMigrationInfoAction { get; set; }
            
            protected override void ConfigureUpgrade(IMigrationBuilder builder)
            {
                ConfigureUpgradeAction(builder);
            }

            protected override void ConfigureDowngrade(IMigrationBuilder builder)
            {
                ConfigureDowngradeAction(builder);
            }

            protected override async Task<MigrationInfo> GetMigrationInfoAsync()
            {
                return await Task.FromResult(GetMigrationInfoFunc());
            }

            protected override async Task UpsertMigrationInfoAsync(MigrationInfo info)
            {
                UpsertMigrationInfoAction(info);
                await Task.CompletedTask;
            }
        }
        #endregion
    }
}