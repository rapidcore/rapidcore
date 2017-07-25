using RapidCore.Environment;
using Xunit;
using Xunit.Abstractions;

namespace RapidCore.UnitTests.Environment
{
    public class EnvironmentVariablesTests
    {
        private readonly EnvironmentVariables envVariables;
        private readonly ITestOutputHelper output;

        public EnvironmentVariablesTests(ITestOutputHelper output)
        {
            this.output = output;
            envVariables = new EnvironmentVariables();
        }

        [Fact]
        public void Get_ReturnsDefault_IfEnvHasNoValue()
        {
            var actual = envVariables.Get<string>("some_key_that_definitely_does_not_exist", "the glorious default");

            Assert.Equal("the glorious default", actual);
        }

        [Fact]
        public void Get_ReturnsValue_ifExists()
        {
            var actual = envVariables.Get<string>("DOTNET_CLI_TELEMETRY_SESSIONID", "default");
            if (actual == "default")
            {
                this.output.WriteLine("No environment variables defined, skipping test");
                return;
            }
            var expected = System.Environment.GetEnvironmentVariable("DOTNET_CLI_TELEMETRY_SESSIONID");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AllSorted()
        {
            var actual = envVariables.AllSorted();
            if (actual.Count == 0)
            {
                this.output.WriteLine("No environment variables defined, skipping test");
                return;
            }

            Assert.True(actual.Count > 0, "There should be at least 1 variable defined");
            Assert.True(actual.ContainsKey("DOTNET_CLI_TELEMETRY_SESSIONID"), "As a minimum 'DOTNET_CLI_TELEMETRY_SESSIONID' should be defined");
        }
    }
}