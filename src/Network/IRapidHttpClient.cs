using System.Net.Http;
using System.Threading.Tasks;

namespace RapidCore.Network
{
    /// <summary>
    /// Layer on top of <see cref="HttpClient"/> to allow for
    /// easy-to-understand mocking of responses.
    /// </summary>
    public interface IRapidHttpClient
    {
        /// <summary>
        /// Send an async request
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>The response</returns>
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}