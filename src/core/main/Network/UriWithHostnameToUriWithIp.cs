using System;
using System.Threading.Tasks;

namespace RapidCore.Network
{
    public class UriWithHostnameToUriWithIp
    {
        private readonly HostnameToIpResolver resolver;

        public UriWithHostnameToUriWithIp(HostnameToIpResolver resolver)
        {
            this.resolver = resolver;
        }

        protected UriWithHostnameToUriWithIp()
        {
            // To allow mocking
        }

        /// <summary>
        /// This method takes in a uri that contains a hostname, resolves the ip and returns the correspoding uri with the ip injected
        ///
        /// I.e mongodb://my-mongo-server:27017 => mongodb://10.1.1.42:27017
        ///
        /// This is to workaround this bug in dotnet core:
        /// https://github.com/dotnet/corefx/issues/8768
        /// </summary>
        /// <param name="uriString">The uri with a hostname to fix</param>
        /// <returns>The patched uri with an ip instead of a hostname</returns>
        public virtual async Task<string> ConvertAsync(string uriString)
        {
            if (!uriString.Contains("://"))
            {
                uriString = $"fakeit://{uriString}";
            }

            var uri = new Uri(uriString);
            var ipaddr = await resolver.ResolveToIpv4Async(uri.DnsSafeHost);

            return uri.OriginalString.Replace(uri.Host, ipaddr).Replace("fakeit://", string.Empty);
        }

        /// <summary>
        /// Synchronously convert a uri that contains a hostname, resolves the ip and returns the correspoding uri with the ip injected
        ///
        /// I.e mongodb://my-mongo-server:27017 => mongodb://10.1.1.42:27017
        ///
        /// This is to workaround this bug in dotnet core:
        /// https://github.com/dotnet/corefx/issues/8768
        /// </summary>
        /// <param name="uriString">The uri with a hostname to fix</param>
        /// <returns>The patched uri with an ip instead of a hostname</return>
        public virtual string Convert(string uriString)
        {
            return ConvertAsync(uriString).Result;
        }
    }
}
