# Http clients

When building systems that contact external 3rd party systems, having a way to mock the requests in unit tests is important, but it is also very useful to be able to mock the requests when running the system in a non-production environment as this provides a way to do full flow testing.

This is where `RapidCore.Network.IRapidHttpClient` comes in. It is a wrapper around `System.Net.Http.HttpClient`.

RapidCore contains 2 implementations:

1. `RapidCore.Network.RealRapidHttpClient`, which simply delegates all work to `System.Net.Http.HttpClient`
2. `RapidCore.Network.MockRapidHttpClient`, which maps a given request to a response, using the test cases it has been provided

The case this is meant to support, is that your application needs to talk to an external service somewhere using HTTP. In production this is easy, but in development you might not have those services available (at least not consistently), but you want to be able to run "full-flow" tests with predictable responses. You also do not want to have to make special test code deep in the belly of your codebase.

Our suggestion is that you do something like this:

```csharp
using RapidCore.Network;

class ExternalServiceClient
{
    public ExternalServiceClient(IRapidHttpClient httpClient) { ... }

    public async Task<...> GetSomethingSpecific(string someId)
    {
        var request = new HttpRequestMessage($"http://example.com/{someId}");
        var response = await httpClient.SendAsync(request);
        ....
    }
}

class ExampleDotComTestCase : IMockRapidHttpClientTestCase
{
    public bool IsMatch(HttpRequestMessage request)
    {
        // look for requests for example.com
        return request.RequestUri.Host.Equals("example.com");
    }

    public Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
    {
        // always respond with "bad gateway"
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadGateway));
    }
}

class ContainerConfiguration
{
    if (config.UseHttpMocking)
    {
        var mockClient = new MockRapidHttpClient();
        mockClient.AddTestCase(new ExampleDotComTestCase());

        container.RegisterAs<IRapidHttpClient>(mockClient);
    }
    else
    {
        container.RegisterAs<IRapidHttpClient>(new RealRapidHttpClient(new HttpClient()));
    }
}
```
