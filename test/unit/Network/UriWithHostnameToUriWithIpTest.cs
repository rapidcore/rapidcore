using System.Threading.Tasks;
using FakeItEasy;
using RapidCore.Network;
using Xunit;

namespace RapidCore.UnitTests.Network
{
    public class UriWithHostnameToUriWithIpTest
    {
        private readonly HostnameToIpResolver _resolver;

        public UriWithHostnameToUriWithIpTest()
        {
            _resolver = A.Fake<HostnameToIpResolver>();
            A.CallTo(() => _resolver.ResolveToIpv4Async(A<string>._)).Returns(Task.FromResult("127.0.0.1"));
        }

        [Theory]
        [InlineData("mongodb://my-mongo:27017", "mongodb://127.0.0.1:27017")]
        [InlineData("redis://the-redis:27017", "redis://127.0.0.1:27017")]
        [InlineData("the-redis:27017", "127.0.0.1:27017")]
        [InlineData("mongodb://user:pass@my-mongo:27017", "mongodb://user:pass@127.0.0.1:27017")]
        public async Task HostnameToIp_just_works_async(string toFix, string expectedFix)
        {
            var fixer = new UriWithHostnameToUriWithIp(_resolver);

            var actual = await fixer.ConvertAsync(toFix);
            Assert.Equal(expectedFix, actual);
        }

        [Theory]
        [InlineData("mongodb://my-mongo:27017", "mongodb://127.0.0.1:27017")]
        [InlineData("redis://the-redis:27017", "redis://127.0.0.1:27017")]
        [InlineData("the-redis:27017", "127.0.0.1:27017")]
        [InlineData("mongodb://user:pass@my-mongo:27017", "mongodb://user:pass@127.0.0.1:27017")]
        public void HostnameToIp_works_sync_too(string toFix, string expectedFix)
        {
            var fixer = new UriWithHostnameToUriWithIp(_resolver);

            var actual = fixer.Convert(toFix);
            Assert.Equal(expectedFix, actual);
        }
    }
}
