using System;
using RapidCore.Security;
using Xunit;

namespace UnitTests.Core.Security
{
    public class RandomNumberGeneratorGuidTest
    {
        private readonly RandomNumberGeneratorGuid _guidGenerator;

        public RandomNumberGeneratorGuidTest()
        {
            _guidGenerator = new RandomNumberGeneratorGuid();
        }

        [Fact]
        public void Can_GenerateGuid()
        {
            var guid = _guidGenerator.GenerateGuid();

            Assert.NotNull(guid);
            Assert.IsType(typeof(Guid), guid);
            Assert.NotEqual(Guid.Empty, guid);
        }
    }
}
