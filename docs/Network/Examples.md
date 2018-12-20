# Examples

## How to get IP/DNS from a hostname

This is a mockable wrapper around `System.Net.Dns`.

```csharp
using RapidCore.Network;

var resolver = new HostnameToIpResolver();

string ip = await resolver.ResolveToIpV4Async("my-mongo-server");
// => 10.1.1.42
```


## How to get URI with IP from URI with hostname

The `RapidCore.Network.UriWithHostnameToUriWithIp` exists in order to work around [issue 8768 in Core 1.0](https://github.com/dotnet/corefx/issues/8768), which meant that we had to resolve hostnames to IPs in connection strings.

E.g. `mongodb://my-mongo-server:27017 => mongodb://10.1.1.42:27017`.

The methods also exist in a synchronous version.

```csharp
using RapidCore.Network;

var mapper = new UriWithHostnameToUriWithIp(new HostnameToIpResolver());

await mapper.ConvertAsync("my-mongo-server:27017"); // => 10.1.1.42:27017
await mapper.ConvertAsync("mongodb://my-mongo-server:27017"); // => mongodb://10.1.1.42:27017
```

## How to mock the request in unit test

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
