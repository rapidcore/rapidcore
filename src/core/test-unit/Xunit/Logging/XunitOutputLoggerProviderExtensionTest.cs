using FakeItEasy;
using Microsoft.Extensions.Logging;
using RapidCore.Xunit.Logging;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Xunit.Logging
{
    public class XunitOutputLoggerProviderExtensionTest
    {
        private ITestOutputHelper _fakeOutput;
        private ILoggerFactory _fakeLoggerFactory;

        public XunitOutputLoggerProviderExtensionTest()
        {
            _fakeOutput = A.Fake<ITestOutputHelper>();
            _fakeLoggerFactory = A.Fake<ILoggerFactory>();
        }

        [Fact]
        public void ProviderExtension_attaches_logger_to_factory()
        {
            // act
            XunitOutputLoggerProviderExtension.AddXunitOutput(_fakeLoggerFactory, _fakeOutput);
            
            // assert
            A.CallTo(() =>
                    _fakeLoggerFactory.AddProvider(A<ILoggerProvider>.That.IsInstanceOf(typeof(XUnitOutputLoggerProvider))))
                .MustHaveHappened();
        }
    }
}
