[![Build status](https://ci.appveyor.com/api/projects/status/0a9kh4v7a6py068e?svg=true)](https://ci.appveyor.com/project/nover/rapidcore.xunit/branch/master)
[![nuget version][nuget-image]][nuget-url]

# RapidCore xunit

`RapidCore.Xunit` contains helper stuff for writing xunit tests. 

Current features

- Provides an `ILoggerFactory` extension for adding an `ILogger` that writes to the xunit output buffer. Simply issue: `LogFactory.AddXunitOutput(output)` passing it the `ITestOutputHelper` instance

## Versioning

We use [SemVer](http://semver.org/), which means you can depend on RapidCore.Xunit version `<major>.*`

It also means that while the major version is `0`, the APIs should be considered unstable.

## Issues

Issues should be reported in https://github.com/rapidcore/issues/issues

[nuget-image]: https://img.shields.io/nuget/v/RapidCore.Xunit.svg
[nuget-url]: https://www.nuget.org/packages/RapidCore.Xunit
