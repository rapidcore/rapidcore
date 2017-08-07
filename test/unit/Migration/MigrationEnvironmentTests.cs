using RapidCore.Mongo.Migration;
using Xunit;

namespace RapidCore.Mongo.UnitTests.Migration
{
    public class MigrationEnvironmentTests
    {
        [Fact]
        public void IsTesting_works_caseInsensitive()
        {
            Assert.True(new MigrationEnvironment("teSTiNg").IsTesting());
        }
        
        [Fact]
        public void IsStaging_works_caseInsensitive()
        {
            Assert.True(new MigrationEnvironment("STAging").IsStaging());
        }
        
        [Fact]
        public void IsProduction_works_caseInsensitive()
        {
            Assert.True(new MigrationEnvironment("ProDUCtion").IsProduction());
        }
        
        [Fact]
        public void Environment_Works()
        {
            Assert.Equal("das environment", new MigrationEnvironment("das environment").Environment);
        }
    }
}