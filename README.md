[![Number of downloads][nuget-image-core-downloads]][nuget-url-core]
[![Build status][appveyor-image]][appveyor-url]
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=rapidcore_rapidcore&metric=alert_status)][sonarcloud-url]

# RapidCore

Library with classes for the stuff you would otherwise build in every single project.

We target `NetStandard2.0` to include as many .NET runtimes as possible, while still having access to as many features as possible (see [Microsoft's version matrix](https://github.com/dotnet/standard/blob/master/docs/versions.md)).

Go to [documentation](http://docs.rapidcore.io/) - preview features are available on the [preview docs site](http://preview-docs.rapidcore.io/)

## Versioning

We use [SemVer](http://semver.org/), which means you can depend on RapidCore version `<major>.*`

It also means that while the major version is `0`, the APIs should be considered unstable.

## Packages

The library is split into multiple packages (based on external dependencies).

### RapidCore (the base library)

[![nuget version][nuget-image-core]][nuget-url-core] [![nuget version][nuget-image-core-pre]][nuget-url-core]

### RapidCore.GoogleCloud

RapidCore classes for [Google Cloud Platform](https://cloud.google.com) that depend on various SDKs.

[![nuget version][nuget-image-google-cloud]][nuget-url-google-cloud] [![nuget version][nuget-image-google-cloud-pre]][nuget-url-google-cloud]

### RapidCore.Mongo

RapidCore classes that depend on MongoDB.

[![nuget version][nuget-image-mongo]][nuget-url-mongo] [![nuget version][nuget-image-mongo-pre]][nuget-url-mongo]

### RapidCore.PostgreSql

RapidCore classes that depend on PostgreSql.

[![nuget version][nuget-image-postgres]][nuget-url-postgres] [![nuget version][nuget-image-postgres-pre]][nuget-url-postgres]

### RapidCore.SqlServer

RapidCore classes that depend on SqlServer.

[![nuget version][nuget-image-sqlserver]][nuget-url-sqlserver] [![nuget version][nuget-image-sqlserver-pre]][nuget-url-sqlserver]

### RapidCore.Redis

RapidCore classes that depend on Redis.

[![nuget version][nuget-image-redis]][nuget-url-redis] [![nuget version][nuget-image-redis-pre]][nuget-url-redis]

### RapidCore.Xunit

Contains helper stuff for writing xunit tests.

Current features

- Provides an `ILoggerFactory` extension for adding an `ILogger` that writes to the xunit output buffer. Simply issue: `LogFactory.AddXunitOutput(output)` passing it the `ITestOutputHelper` instance

[![nuget version][nuget-image-xunit]][nuget-url-xunit] [![nuget version][nuget-image-xunit-pre]][nuget-url-xunit]


[nuget-image-core]: https://img.shields.io/nuget/v/RapidCore.svg?style=flat-square
[nuget-image-core-downloads]: https://img.shields.io/nuget/dt/RapidCore.svg?style=flat-square
[nuget-image-core-pre]: https://img.shields.io/nuget/vpre/RapidCore.svg?style=flat-square
[nuget-url-core]: https://www.nuget.org/packages/RapidCore

[nuget-image-google-cloud]: https://img.shields.io/nuget/v/RapidCore.GoogleCloud.svg?style=flat-square
[nuget-image-google-cloud-pre]: https://img.shields.io/nuget/vpre/RapidCore.GoogleCloud.svg?style=flat-square
[nuget-url-google-cloud]: https://www.nuget.org/packages/RapidCore.GoogleCloud

[nuget-image-mongo]: https://img.shields.io/nuget/v/RapidCore.Mongo.svg?style=flat-square
[nuget-image-mongo-pre]: https://img.shields.io/nuget/vpre/RapidCore.Mongo.svg?style=flat-square
[nuget-url-mongo]: https://www.nuget.org/packages/RapidCore.Mongo

[nuget-image-postgres]: https://img.shields.io/nuget/v/RapidCore.PostgreSql.svg?style=flat-square
[nuget-image-postgres-pre]: https://img.shields.io/nuget/vpre/RapidCore.PostgreSql.svg?style=flat-square
[nuget-url-postgres]: https://www.nuget.org/packages/RapidCore.PostgreSql

[nuget-image-redis]: https://img.shields.io/nuget/v/RapidCore.Redis.svg?style=flat-square
[nuget-image-redis-pre]: https://img.shields.io/nuget/vpre/RapidCore.Redis.svg?style=flat-square
[nuget-url-redis]: https://www.nuget.org/packages/RapidCore.Redis

[nuget-image-sqlserver]: https://img.shields.io/nuget/v/RapidCore.SqlServer.svg?style=flat-square
[nuget-image-sqlserver-pre]: https://img.shields.io/nuget/vpre/RapidCore.SqlServer.svg?style=flat-square
[nuget-url-sqlserver]: https://www.nuget.org/packages/RapidCore.SqlServer

[nuget-image-xunit]: https://img.shields.io/nuget/v/RapidCore.Xunit.svg?style=flat-square
[nuget-image-xunit-pre]: https://img.shields.io/nuget/vpre/RapidCore.Xunit.svg?style=flat-square
[nuget-url-xunit]: https://www.nuget.org/packages/RapidCore.Xunit


[appveyor-image]: https://img.shields.io/appveyor/ci/nover/rapidcore/master.svg?style=flat-square
[appveyor-url]: https://ci.appveyor.com/project/nover/rapidcore/branch/master

[sonarcloud-url]: https://sonarcloud.io/dashboard?id=rapidcore_rapidcore

## Notes for devs

Start by running `docker-compose up -d`.

### Documentation

The documentation is available with live-reload at http://localhost:8000.

### SonarCloud

We scan the code using [SonarCloud](https://sonarcloud.io/dashboard?id=rapidcore_rapidcore).
