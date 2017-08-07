using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using RapidCore.Mongo.Migration;
using RapidCore.Mongo.Migration.Internal;
using Xunit;

namespace RapidCore.Mongo.UnitTests.Migration
{
    public class MigrationBaseTest
    {
        private readonly MigrationBase _migration;

        public MigrationBaseTest()
        {
            // Create a "fake" instance of the base class - all non abtract and non virual functions will remain, so we can test them!
            _migration = A.Fake<MigrationBase>();
        }

        [Fact]
        public async Task MigrationBase_get_pending_steps_work_with_nothing_in_mongodb()
        {
            var mongoDbConnection = A.Fake<MongoDbConnection>();
            A.CallTo(() => mongoDbConnection.FirstOrDefaultAsync(MigrationDocument.CollectionName,
                    A<Expression<Func<MigrationDocument, bool>>>.Ignored))
                .Returns(Task.FromResult(default(MigrationDocument)));
            var connectionProvider = A.Fake<IConnectionProvider>();

            A.CallTo(() => connectionProvider.Default()).Returns(mongoDbConnection);
            var builder = A.Fake<IMigrationBuilder>();
            A.CallTo(() => builder.GetAllStepNames()).Returns(new List<string>
            {
                "step1",
                "step2",
                "step3"
            });

            (var pending, var doc) = await _migration.GetPendingStepsAsync(builder, connectionProvider);
            Assert.NotNull(pending);
            Assert.True(pending.Count == 3);
            Assert.Contains("step1", pending);
            Assert.Contains("step2", pending);
            Assert.Contains("step3", pending);
        }

        [Fact]
        public async Task MigrationBase_get_pending_steps_work_with_partially_applied_migration()
        {
            var mongoDbConnection = A.Fake<MongoDbConnection>();
            A.CallTo(() => mongoDbConnection.FirstOrDefaultAsync(MigrationDocument.CollectionName,
                    A<Expression<Func<MigrationDocument, bool>>>.Ignored))
                .Returns(Task.FromResult(new MigrationDocument
                {
                    Name = "something",
                    MigrationCompleted = false,
                    StepsCompleted = new List<string>
                    {
                        "step1"
                    }
                }));
            var connectionProvider = A.Fake<IConnectionProvider>();

            A.CallTo(() => connectionProvider.Default()).Returns(mongoDbConnection);
            var builder = A.Fake<IMigrationBuilder>();
            A.CallTo(() => builder.GetAllStepNames()).Returns(new List<string>
            {
                "step1",
                "step2",
                "step3"
            });

            (var pending, var doc) = await _migration.GetPendingStepsAsync(builder, connectionProvider);
            Assert.NotNull(pending);
            Assert.True(pending.Count == 2);
            Assert.Contains("step2", pending);
            Assert.Contains("step3", pending);
        }

        [Fact]
        public async Task MigrationBase_upgrade_async_works()
        {
            var mongoDbConnection = A.Fake<MongoDbConnection>(opts => opts.Strict());
            A.CallTo(() => mongoDbConnection.FirstOrDefaultAsync(MigrationDocument.CollectionName,
                    A<Expression<Func<MigrationDocument, bool>>>.Ignored))
                .Returns(Task.FromResult(default(MigrationDocument)));

            MigrationDocument upsertedDocument = null;
            A.CallTo(() => mongoDbConnection.UpsertAsync(
                    MigrationDocument.CollectionName,
                    A<MigrationDocument>.Ignored,
                    A<Expression<Func<MigrationDocument, bool>>>.Ignored))
                .Invokes(call => upsertedDocument = (MigrationDocument)call.Arguments[1]);

            var connectionProvider = A.Fake<IConnectionProvider>(opts => opts.Strict());
            A.CallTo(() => connectionProvider.Default()).Returns(mongoDbConnection);

            var step1Invoked = false;
            var step2Invoked = false;
            A.CallTo(() => _migration.ConfigureUpgrade(A<MigrationBuilder>.Ignored)).Invokes(call =>
            {
                var builder = (MigrationBuilder)call.Arguments[0];
                builder
                    .WithStep("step1", () => step1Invoked = true)
                    .WithStep("step2", () => step2Invoked = true);
            });

            await _migration.UpgradeAsync(new MigrationContext
            {
                ConnectionProvider = connectionProvider,
            });


            Assert.True(step1Invoked);
            Assert.True(step2Invoked);

            Assert.NotNull(upsertedDocument);
            var stepsCompleted = upsertedDocument.StepsCompleted;
            Assert.True(stepsCompleted.Count == 2, "upsertedDocument.StepsCompleted.Count == 2");
            Assert.Contains("step1", stepsCompleted);
            Assert.Contains("step2", stepsCompleted);
        }

        [Fact]
        public async Task MigrationBase_upgrade_async_saves_partial_state_on_exceptions()
        {
            var mongoDbConnection = A.Fake<MongoDbConnection>(opts => opts.Strict());
            A.CallTo(() => mongoDbConnection.FirstOrDefaultAsync(MigrationDocument.CollectionName,
                    A<Expression<Func<MigrationDocument, bool>>>.Ignored))
                .Returns(Task.FromResult(default(MigrationDocument)));

            MigrationDocument upsertedDocument = null;
            A.CallTo(() => mongoDbConnection.UpsertAsync(
                    MigrationDocument.CollectionName,
                    A<MigrationDocument>.Ignored,
                    A<Expression<Func<MigrationDocument, bool>>>.Ignored))
                .Invokes(call => upsertedDocument = (MigrationDocument)call.Arguments[1]);

            var connectionProvider = A.Fake<IConnectionProvider>(opts => opts.Strict());
            A.CallTo(() => connectionProvider.Default()).Returns(mongoDbConnection);

            var step1Invoked = false;
            A.CallTo(() => _migration.ConfigureUpgrade(A<MigrationBuilder>.Ignored)).Invokes(call =>
            {
                var builder = (MigrationBuilder)call.Arguments[0];
                builder
                    .WithStep("step1", () => step1Invoked = true)
                    .WithStep("step2", () => throw new NotSupportedException("Breaking 'cause I can!"));
            });

            // ReSharper disable once PossibleNullReferenceException
            var migrationException = await Record.ExceptionAsync(async () =>
            await _migration.UpgradeAsync(new MigrationContext
            {
                ConnectionProvider = connectionProvider,
            }));
            Assert.NotNull(migrationException);

            Assert.True(step1Invoked);

            Assert.NotNull(upsertedDocument);
            var stepsCompleted = upsertedDocument.StepsCompleted;
            Assert.True(stepsCompleted.Count == 1, "upsertedDocument.StepsCompleted.Count == 1");
            Assert.Contains("step1", stepsCompleted);
        }
    }
}
