# RapidCore

## Resources

- [Source code](https://github.com/rapidcore/rapidcore) - See and contribute to the source code
- [Report an issue](https://github.com/rapidcore/rapidcore/issues)
- [Request a feature](https://github.com/rapidcore/rapidcore/issues)
- [Release notes](https://github.com/rapidcore/rapidcore/releases)
- [SonarCloud report][sonarcloud-url]

## Our goals

- code should be maintainable
    - readable
    - as simple as possible
- code should be testable 
    - mockable (unit tests should be easy to control)
- exceptions should be helpful
    - types that make sense
    - messages that help you fix/diagnose the problem

## Packages

[![Build status][appveyor-image]][appveyor-url]
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=rapidcore_rapidcore&metric=alert_status)][sonarcloud-url]


| Package | Latest stable | Latest | Downloads |
|---------|---------------|--------|-----------|
| RapidCore | [![latest stable nuget version][core-nuget-image]][core-nuget-url] | [![latest nuget version][core-nuget-image-pre]][core-nuget-url] | [![Number of downloads][core-nuget-image-downloads]][core-nuget-url] |
| RapidCore.GoogleCloud | [![latest stable nuget version][google-cloud-nuget-image]][google-cloud-nuget-url] | [![latest nuget version][google-cloud-nuget-image-pre]][google-cloud-nuget-url] | [![Number of downloads][google-cloud-nuget-image-downloads]][google-cloud-nuget-url] |
| RapidCore.Mongo | [![latest stable nuget version][mongo-nuget-image]][mongo-nuget-url] | [![latest nuget version][mongo-nuget-image-pre]][mongo-nuget-url] | [![Number of downloads][mongo-nuget-image-downloads]][mongo-nuget-url] |
| RapidCore.PostgreSql | [![latest stable nuget version][postgres-nuget-image]][postgres-nuget-url] | [![latest nuget version][postgres-nuget-image-pre]][postgres-nuget-url] | [![Number of downloads][postgres-nuget-image-downloads]][postgres-nuget-url] |
| RapidCore.Redis | [![latest stable nuget version][redis-nuget-image]][redis-nuget-url] | [![latest nuget version][redis-nuget-image-pre]][redis-nuget-url] | [![Number of downloads][redis-nuget-image-downloads]][redis-nuget-url] |
| RapidCore.SqlServer | [![latest stable nuget version][sqlserver-nuget-image]][sqlserver-nuget-url] | [![latest nuget version][sqlserver-nuget-image-pre]][sqlserver-nuget-url] | [![Number of downloads][sqlserver-nuget-image-downloads]][sqlserver-nuget-url] |
| RapidCore.Xunit | [![latest stable nuget version][xunit-nuget-image]][xunit-nuget-url] | [![latest nuget version][xunit-nuget-image-pre]][xunit-nuget-url] | [![Number of downloads][xunit-nuget-image-downloads]][xunit-nuget-url] |


[core-nuget-image]: https://img.shields.io/nuget/v/RapidCore.svg?style=flat-square
[core-nuget-image-downloads]: https://img.shields.io/nuget/dt/RapidCore.svg?style=flat-square
[core-nuget-image-pre]: https://img.shields.io/nuget/vpre/RapidCore.svg?style=flat-square
[core-nuget-url]: https://www.nuget.org/packages/RapidCore

[google-cloud-nuget-image]: https://img.shields.io/nuget/v/RapidCore.GoogleCloud.svg?style=flat-square
[google-cloud-nuget-image-downloads]: https://img.shields.io/nuget/dt/RapidCore.GoogleCloud.svg?style=flat-square
[google-cloud-nuget-image-pre]: https://img.shields.io/nuget/vpre/RapidCore.GoogleCloud.svg?style=flat-square
[google-cloud-nuget-url]: https://www.nuget.org/packages/RapidCore.GoogleCloud

[mongo-nuget-image]: https://img.shields.io/nuget/v/RapidCore.Mongo.svg?style=flat-square
[mongo-nuget-image-downloads]: https://img.shields.io/nuget/dt/RapidCore.Mongo.svg?style=flat-square
[mongo-nuget-image-pre]: https://img.shields.io/nuget/vpre/RapidCore.Mongo.svg?style=flat-square
[mongo-nuget-url]: https://www.nuget.org/packages/RapidCore.Mongo

[postgres-nuget-image]: https://img.shields.io/nuget/v/RapidCore.PostgreSql.svg?style=flat-square
[postgres-nuget-image-downloads]: https://img.shields.io/nuget/dt/RapidCore.PostgreSql.svg?style=flat-square
[postgres-nuget-image-pre]: https://img.shields.io/nuget/vpre/RapidCore.PostgreSql.svg?style=flat-square
[postgres-nuget-url]: https://www.nuget.org/packages/RapidCore.PostgreSql

[redis-nuget-image]: https://img.shields.io/nuget/v/RapidCore.Redis.svg?style=flat-square
[redis-nuget-image-downloads]: https://img.shields.io/nuget/dt/RapidCore.Redis.svg?style=flat-square
[redis-nuget-image-pre]: https://img.shields.io/nuget/vpre/RapidCore.Redis.svg?style=flat-square
[redis-nuget-url]: https://www.nuget.org/packages/RapidCore.Redis

[sqlserver-nuget-image]: https://img.shields.io/nuget/v/RapidCore.SqlServer.svg?style=flat-square
[sqlserver-nuget-image-downloads]: https://img.shields.io/nuget/dt/RapidCore.SqlServer.svg?style=flat-square
[sqlserver-nuget-image-pre]: https://img.shields.io/nuget/vpre/RapidCore.SqlServer.svg?style=flat-square
[sqlserver-nuget-url]: https://www.nuget.org/packages/RapidCore.SqlServer

[xunit-nuget-image]: https://img.shields.io/nuget/v/RapidCore.Xunit.svg?style=flat-square
[xunit-nuget-image-downloads]: https://img.shields.io/nuget/dt/RapidCore.Xunit.svg?style=flat-square
[xunit-nuget-image-pre]: https://img.shields.io/nuget/vpre/RapidCore.Xunit.svg?style=flat-square
[xunit-nuget-url]: https://www.nuget.org/packages/RapidCore.Xunit


[appveyor-image]: https://img.shields.io/appveyor/ci/nover/rapidcore/master.svg?style=flat-square
[appveyor-url]: https://ci.appveyor.com/project/nover/rapidcore/branch/master

[sonarcloud-url]: https://sonarcloud.io/dashboard?id=rapidcore_rapidcore
