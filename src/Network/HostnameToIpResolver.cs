using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace RapidCore.Network
{
    public class HostnameToIpResolver
    {
        /// <summary>
        /// Resolve the given hostname into the first available Ipv4 address
        /// </summary>
        /// <param name="hostname">The hostname to resolve</param>
        /// <returns>The Ipv4 address</returns>
        public virtual async Task<string> ResolveToIpv4Async(string hostname)
        {
            var dnsTask = Dns.GetHostAddressesAsync(hostname);
            var addresses = await dnsTask;
            var connect = addresses.Where(x => x.AddressFamily == AddressFamily.InterNetwork).Select(x => x.MapToIPv4().ToString()).First();

            return connect;
        }
    }
}
