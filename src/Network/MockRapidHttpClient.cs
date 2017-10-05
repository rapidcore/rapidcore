using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RapidCore.Network
{
    /// <summary>
    /// Never actually sends requests anywhere. All responses come
    /// from the provided test cases
    /// </summary>
    public class MockRapidHttpClient : IRapidHttpClient
    {
        private readonly List<IMockRapidHttpClientTestCase> testCases = new List<IMockRapidHttpClientTestCase>();

        /// <summary>
        /// "Send" the request - i.e. find a matching test case and
        /// run it.
        ///
        /// Note that the first test case found that matches the request will be used.
        ///
        /// If you want a default response, simply add a "return true" test case as the
        /// last test case. It will then act as a catch-all.
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>The mock response provided by the matching test case</returns>
        /// <exception cref="MockRapidHttpClientException">Thrown if no test cases matches</exception>
        public virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var testCase = testCases.FirstOrDefault(x => x.IsMatch(request));

            if (testCase == default(IMockRapidHttpClientTestCase))
            {
                throw new MockRapidHttpClientException("Could not find a suitable test-case for the request.", request);
            }

            return testCase.GetResponseAsync(request);
        }

        /// <summary>
        /// Add a test case
        /// </summary>
        /// <param name="testCase">The test case</param>
        public virtual MockRapidHttpClient AddTestCase(IMockRapidHttpClientTestCase testCase)
        {
            testCases.Add(testCase);
            return this;
        }
        
        /// <summary>
        /// The base address of Uniform Resource Identifier (URI) of the Internet resource used when sending requests.
        /// 
        /// <see cref="HttpClient.BaseAddress"/>
        /// </summary>
        public virtual Uri BaseAddress { get; set; }

        /// <summary>
        /// The timespan to wait before the request times out.
        /// 
        /// <see cref="HttpClient.Timeout"/>
        /// </summary>
        public virtual TimeSpan Timeout { get; set; }
    }
}