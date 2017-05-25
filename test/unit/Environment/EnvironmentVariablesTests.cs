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
            var actual = envVariables.Get<string>("PATH", "default");

            var expected = System.Environment.GetEnvironmentVariable("PATH");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AllSorted()
        {
            var actual = envVariables.AllSorted();

            Assert.True(actual.Count > 0);
            Assert.True(actual.ContainsKey("PATH"));
        }
    }
}