using FakeItEasy;
using Microsoft.Extensions.Logging;
using RapidCore.Xunit.Logging;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Xunit.Logging
{
    public class XunitOutputLoggerTest
    {
        private readonly ITestOutputHelper _fakeOutput;
        private readonly XUnitOutputLogger _logger;

        public XunitOutputLoggerTest()
        {
            _fakeOutput = A.Fake<ITestOutputHelper>();
            _logger = new XUnitOutputLogger("cat", _fakeOutput);
        }
        
        [Fact]
        public void Logger_writes_to_output()
        {
            // act
            _logger.Log(LogLevel.Information, 42, "stuff", null, (s, exception) => s);
            
            // asssert
            A.CallTo(() => _fakeOutput.WriteLine(A<string>.Ignored)).MustHaveHappened();

        }

        [Fact]
        public void Logger_enabled_true()
        {
            Assert.True(_logger.IsEnabled(LogLevel.Information));
        }

        [Fact]
        public void Logger_beging_scope_works()
        {
            var scope = _logger.BeginScope("stuff");
            Assert.IsType<NoopDisposable>(scope);
        }
    }
}
