using System;
using System.Threading.Tasks;
using RapidCore.Migration.Internal;
using Xunit;

namespace UnitTests.Core.Migration.Internal
{
    public class MigrationBuilderTest
    {
        private readonly MigrationBuilder _builder;
        private readonly Func<Task> _defaultAction;

        public MigrationBuilderTest()
        {
            _builder = new MigrationBuilder();
            _defaultAction = async () =>
            {
#pragma warning disable 219
                var that = 1 + 1;
#pragma warning restore 219
                await Task.CompletedTask;
            };
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData("\t\r\n    ")]
        [InlineData("    ")]
        [InlineData("\r")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        public void MigrationBuilder_rejects_emptyish_step_names(string stepName)
        {
            var ex = Record.Exception(() => _builder.Step(stepName, _defaultAction));
            Assert.NotNull(ex);

            var nullException = Assert.IsType<ArgumentNullException>(ex);
            Assert.Equal("stepName", nullException.ParamName);
        }

        [Fact]
        public void MigrationBuilder_rejects_null_actions()
        {
            var ex = Record.Exception(() => _builder.Step("test", null));
            Assert.NotNull(ex);
            var argumentException = Assert.IsType<ArgumentNullException>(ex);

            Assert.Equal("action", argumentException.ParamName);
        }
        [Fact]
        public void MigrationBuilder_add_step_works()
        {
            _builder.Step("this-step", _defaultAction);

            Assert.True(_builder.MigrationSteps.Count == 1, "builder.MigrationSteps.Count == 1");
        }

        [Fact]
        public void MigrationBuilder_add_step_in_chains_work()
        {
            _builder
                .Step("this-step", _defaultAction)
                .Step("that-step", _defaultAction)
                .Step("also-this", _defaultAction);

            Assert.True(_builder.MigrationSteps.Count == 3, "builder.MigrationSteps.Count == 3");
        }
        [Fact]
        public void MigrationBuilder_get_all_step_names()
        {
            _builder
                .Step("this-step", _defaultAction)
                .Step("that-step", _defaultAction)
                .Step("also-this", _defaultAction);

            var addedSteps = _builder.GetAllStepNames();
            Assert.True(addedSteps.Count == 3, "addedSteps.Count == 3");
            Assert.Contains("this-step", addedSteps);
            Assert.Contains("that-step", addedSteps);
            Assert.Contains("also-this", addedSteps);
        }

        [Fact]
        public void MigrationBuilder_enforces_unique_step_names()
        {
            _builder.Step("this-step", _defaultAction);

            var ex = Record.Exception(() => _builder.Step("this-step", _defaultAction));
            Assert.NotNull(ex); // there must be an exception thrown
            var argumentException = Assert.IsType<ArgumentException>(ex);

            Assert.Equal("stepName", argumentException.ParamName);
            Assert.True(_builder.MigrationSteps.Count == 1, "_builder.MigrationSteps.Count == 1");
        }

        [Fact]
        public void MigrationBuilder_get_action_for_step_works()
        {
            const string stepName = "this-step";
            _builder.Step(stepName, _defaultAction);

            var act = _builder.GetActionForMigrationStep(stepName);
            Assert.NotNull(act);
        }

        [Fact]
        public void MigrationBuilder_get_action_for_step_can_throw()
        {
            const string stepName = "this-step";
            _builder.Step(stepName, _defaultAction);

            var ex = Record.Exception(() =>  _builder.GetActionForMigrationStep("this was not added!"));
            Assert.NotNull(ex);
            var argumentException = Assert.IsType<ArgumentException>(ex);
            Assert.Equal("stepName", argumentException.ParamName);
        }
    }
}
