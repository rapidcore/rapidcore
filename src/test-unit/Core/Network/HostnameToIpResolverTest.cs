using System.Net.Sockets;
using System.Threading.Tasks;
using RapidCore.Network;
using Xunit;

namespace UnitTests.Core.Network
{
    public class HostnameToIpResolverTest
    {
        [Fact]
        public async Task HostnameToIpResolver_can_resolveAsync()
        {
            var resolver = new HostnameToIpResolver();
            var ip = await resolver.ResolveToIpv4Async("localhost");

            Assert.Equal("127.0.0.1", ip);
        }

        [Fact]
        public async Task HostnameToIpResolve_can_fail()
        {
            var resolver = new HostnameToIpResolver();
            var exception =
                await Record.ExceptionAsync(async () => await resolver.ResolveToIpv4Async("this-host-is-invalid"));
            Assert.NotNull(exception);
            
            Assert.IsAssignableFrom<SocketException>(exception);
        }
    }
}
