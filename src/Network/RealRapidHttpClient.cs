using System.Net.Http;
using System.Threading.Tasks;

namespace RapidCore.Network
{
    /// <summary>
    /// Actually sends request - no mocking.
    /// </summary>
    public class RealRapidHttpClient : IRapidHttpClient
    {
        private readonly HttpClient httpClient;

        public RealRapidHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Send the request
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>The response</returns>
        public virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return httpClient.SendAsync(request);
        }
    }
}