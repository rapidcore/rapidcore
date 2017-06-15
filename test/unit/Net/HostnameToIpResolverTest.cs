using System;
using System.Collections.Generic;
using System.Text;
using RapidCore.Net;
using Xunit;

namespace RapidCore.UnitTests.Net
{
    public class HostnameToIpResolverTest
    {
        [Fact]
        public async System.Threading.Tasks.Task HostnameToIpResolver_can_resolveAsync()
        {
            var resolver = new HostnameToIpResolver();
            var ip = await resolver.ResolveToIpv4Async("localhost");

            Assert.Equal("127.0.0.1", ip);
        }
    }
}
