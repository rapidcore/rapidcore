# Hostname -> IP aka DNS

## HostnameToIpResolver

This is a mockable wrapper around `System.Net.Dns`.

```csharp
using RapidCore.Network;

var resolver = new HostnameToIpResolver();

string ip = await resolver.ResolveToIpV4Async("my-mongo-server");
// => 10.1.1.42
```


## UriWithHostnameToUriWithIp

The `RapidCore.Network.UriWithHostnameToUriWithIp` exists in order to work around [issue 8768 in Core 1.0](https://github.com/dotnet/corefx/issues/8768), which meant that we had to resolve hostnames to IPs in connection strings.

E.g. `mongodb://my-mongo-server:27017 => mongodb://10.1.1.42:27017`.

The methods also exist in a synchronous version.

```csharp
using RapidCore.Network;

var mapper = new UriWithHostnameToUriWithIp(new HostnameToIpResolver());

await mapper.ConvertAsync("my-mongo-server:27017"); // => 10.1.1.42:27017
await mapper.ConvertAsync("mongodb://my-mongo-server:27017"); // => mongodb://10.1.1.42:27017
```
