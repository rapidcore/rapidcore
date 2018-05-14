[![Build status](https://ci.appveyor.com/api/projects/status/ja3vf8fp1ros6q4t/branch/master?svg=true)](https://ci.appveyor.com/project/nover/rapidcore/branch/master)

# RapidCore

Library with classes for the stuff you would otherwise build in every single project.

We target `NetStandard1.6` to include as many .NET runtimes as possible, while still having access to as many features as possible (see [Microsoft's version matrix](https://github.com/dotnet/standard/blob/master/docs/versions.md)).

Go to [documentation](http://docs.rapidcore.io/)

## Versioning

We use [SemVer](http://semver.org/), which means you can depend on RapidCore version `<major>.*`

It also means that while the major version is `0`, the APIs should be considered unstable.

## Packages

The library is split into multiple packages (based on external dependencies).

### RapidCore (the base library)

[![nuget version][nuget-image-core]][nuget-url-core]

### RapidCore.Mongo

RapidCore classes that depend on MongoDB.

[![nuget version][nuget-image-mongo]][nuget-url-mongo]

### RapidCore.PostgreSql

RapidCore classes that depend on PostgreSql.

[![nuget version][nuget-image-postgres]][nuget-url-postgres]

### RapidCore.Redis

RapidCore classes that depend on Redis.

[![nuget version][nuget-image-redis]][nuget-url-redis]

### RapidCore.Xunit

Contains helper stuff for writing xunit tests.

Current features

- Provides an `ILoggerFactory` extension for adding an `ILogger` that writes to the xunit output buffer. Simply issue: `LogFactory.AddXunitOutput(output)` passing it the `ITestOutputHelper` instance

[![nuget version][nuget-image-xunit]][nuget-url-xunit]


[nuget-image-core]: https://img.shields.io/nuget/v/RapidCore.svg
[nuget-url-core]: https://www.nuget.org/packages/RapidCore

[nuget-image-mongo]: https://img.shields.io/nuget/v/RapidCore.Mongo.svg
[nuget-url-mongo]: https://www.nuget.org/packages/RapidCore.Mongo

[nuget-image-postgres]: https://img.shields.io/nuget/v/RapidCore.PostgreSql.svg
[nuget-url-postgres]: https://www.nuget.org/packages/RapidCore.PostgreSql

[nuget-image-redis]: https://img.shields.io/nuget/v/RapidCore.Redis.svg
[nuget-url-redis]: https://www.nuget.org/packages/RapidCore.Redis

[nuget-image-xunit]: https://img.shields.io/nuget/v/RapidCore.Xunit.svg
[nuget-url-xunit]: https://www.nuget.org/packages/RapidCore.Xunit

## For devs

The documentation is available with live-reload at http://localhost:8000.