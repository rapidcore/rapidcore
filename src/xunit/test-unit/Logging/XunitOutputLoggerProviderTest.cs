using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using RapidCore.Xunit.Logging;
using Xunit;
using Xunit.Abstractions;

namespace RapidCore.Xunit.UnitTest.Logging
{
    public class XunitOutputLoggerProviderTest
    {
        private ITestOutputHelper _fakeOutput;
        private XUnitOutputLoggerProvider _provider;

        public XunitOutputLoggerProviderTest()
        {
            _fakeOutput = A.Fake<ITestOutputHelper>();
            _provider = new XUnitOutputLoggerProvider(_fakeOutput);
        }

        [Fact]
        public void Provider_returns_new_logger()
        {
            var logger = _provider.CreateLogger("cat");

            var typedLogger = Assert.IsType<XUnitOutputLogger>(logger);
            
            Assert.Equal("cat", typedLogger.CategoryName);
        }
    }
}