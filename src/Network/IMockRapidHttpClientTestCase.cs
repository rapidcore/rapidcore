using System.Net.Http;
using System.Threading.Tasks;

namespace RapidCore.Network
{
    /// <summary>
    /// Defines the interface for test cases
    /// used by <see cref="MockRapidHttpClient"/>
    /// </summary>
    public interface IMockRapidHttpClientTestCase
    {
        /// <summary>
        /// Is this the request we are looking for?
        /// </summary>
        /// <param name="request">The request to check</param>
        /// <returns><c>True</c> if the request matches this case, <c>false</c> otherwise.</returns>
        bool IsMatch(HttpRequestMessage request);

        /// <summary>
        /// Get the mock response for the given request.
        ///
        /// This method is async, to allow for cases where you want
        /// to delay the response or some other clever thing :)
        /// </summary>
        /// <param name="request">The request to "respond" to</param>
        /// <returns>The response</returns>
        Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request);
    }
}