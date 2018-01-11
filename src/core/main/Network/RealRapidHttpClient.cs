using System;
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

        /// <summary>
        /// The base address of Uniform Resource Identifier (URI) of the Internet resource used when sending requests.
        /// 
        /// <see cref="HttpClient.BaseAddress"/>
        /// </summary>
        public virtual Uri BaseAddress
        {
            get => httpClient.BaseAddress;
            set => httpClient.BaseAddress = value;
        }

        /// <summary>
        /// The timespan to wait before the request times out.
        /// 
        /// <see cref="HttpClient.Timeout"/>
        /// </summary>
        public virtual TimeSpan Timeout
        {
            get => httpClient.Timeout;
            set => httpClient.Timeout = value;
        }
    }
}