using System;
using System.Net.Http;

namespace RapidCore.Network
{
    public class MockRapidHttpClientException : Exception
    {
        public MockRapidHttpClientException(string message, HttpRequestMessage request)
            : base(message)
        {
            Request = request;
        }

        public HttpRequestMessage Request { get; set; }
    }
}