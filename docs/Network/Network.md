# Network

Network related utility classes.

- `DNS` ([see docs](#dns))
- `Http clients` ([see docs](#http-clients))

### DNS

Helper class to resolve IP/DNS from hostname.

Examples

- [How to get IP/DNS from a hostname](../Examples#how-to-get-ipdns-from-a-hostname)
- [How to get URI with IP from URI with hostname](../Examples#how-to-get-uri-with-ip-from-uri-with-hostname)

### Mockable Http clients

When building systems that contact external 3rd party systems, having a way to mock the requests in unit tests is important, but it is also very useful to be able to mock the requests when running the system in a non-production environment as this provides a way to do full flow testing.

This is where `RapidCore.Network.IRapidHttpClient` comes in. It is a wrapper around `System.Net.Http.HttpClient`.

RapidCore contains 2 implementations:

1. `RapidCore.Network.RealRapidHttpClient`, which simply delegates all work to `System.Net.Http.HttpClient`
2. `RapidCore.Network.MockRapidHttpClient`, which maps a given request to a response, using the test cases it has been provided

The case this is meant to support, is that your application needs to talk to an external service somewhere using HTTP. In production this is easy, but in development you might not have those services available (at least not consistently), but you want to be able to run "full-flow" tests with predictable responses. You also do not want to have to make special test code deep in the belly of your codebase.

Examples

- [How to mock the request in unit test](../Examples#how-to-mock-the-request-in-unit-test)
