using RapidCore.Environment;
using Xunit;

namespace RapidCore.UnitTests.Environment
{
    public class EnvironmentVariablesTests
    {
        private readonly EnvironmentVariables envVariables;

        public EnvironmentVariablesTests()
        {
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

            var expected = System.Environment.GetEnvironmentVariable("DOTNET_CLI_TELEMETRY_SESSIONID");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AllSorted()
        {
            var actual = envVariables.AllSorted();

            Assert.True(actual.Count > 0, "There should be at least 1 variable defined");
            Assert.True(actual.ContainsKey("DOTNET_CLI_TELEMETRY_SESSIONID"), "As a minimum 'DOTNET_CLI_TELEMETRY_SESSIONID' should be defined");
        }
    }
}