using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using RapidCore.Network;
using Xunit;

namespace RapidCore.UnitTests.Network
{
    public class MockRapidHttpClientTests
    {
        private readonly MockRapidHttpClient client;
        private readonly IMockRapidHttpClientTestCase testCase1;
        private readonly IMockRapidHttpClientTestCase testCase2;
        private readonly IMockRapidHttpClientTestCase testCase3;

        public MockRapidHttpClientTests()
        {
            testCase1 = A.Fake<IMockRapidHttpClientTestCase>();
            testCase2 = A.Fake<IMockRapidHttpClientTestCase>();
            testCase3 = A.Fake<IMockRapidHttpClientTestCase>();
            
            client = new MockRapidHttpClient();
        }

        [Fact]
        public async void SendAsync_ThrowsIf_NoTestCasesAtAll()
        {
            var request = new HttpRequestMessage();
            
            var actual = await Assert.ThrowsAsync<MockRapidHttpClientException>(async () =>
                await client.SendAsync(request));
            
            Assert.Same(request, actual.Request);
        }
        
        [Fact]
        public async void SendAsync_ThrowsIf_noMatchingTestCases()
        {
            var request = new HttpRequestMessage();
            client
                .AddTestCase(testCase1)
                .AddTestCase(testCase2);

            A.CallTo(() => testCase1.IsMatch(request)).Returns(false);
            A.CallTo(() => testCase2.IsMatch(request)).Returns(false);
            
            var actual = await Assert.ThrowsAsync<MockRapidHttpClientException>(async () =>
                await client.SendAsync(request));
            
            Assert.Same(request, actual.Request);
        }
        
        [Fact]
        public async void SendAsync_returnsResponse_fromFirstMatchingTestCase()
        {
            var request = new HttpRequestMessage();
            client
                .AddTestCase(testCase1)
                .AddTestCase(testCase2)
                .AddTestCase(testCase3);

            A.CallTo(() => testCase1.IsMatch(request)).Returns(false);
            A.CallTo(() => testCase2.IsMatch(request)).Returns(true);
            A.CallTo(() => testCase3.IsMatch(request)).Returns(true);
            
            var response = new HttpResponseMessage();
            A.CallTo(() => testCase2.GetResponseAsync(request)).Returns(Task.FromResult(response));
            
            var actual = await client.SendAsync(request);
            
            Assert.Same(response, actual);
        }
    }
}