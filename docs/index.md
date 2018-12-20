# RapidCore

Library with classes for the stuff you would otherwise build in every single project.

We target `NetStandard1.6` to include as many .NET runtimes as possible, while still having access to as many features as possible (see [Microsoft's version matrix](https://github.com/dotnet/standard/blob/master/docs/versions.md)).

Go to [documentation](http://docs.rapidcore.io/) - preview features are available on the [preview docs site](http://preview-docs.rapidcore.io/)

## Packages

The library is split into multiple packages (based on external dependencies).

[![Build status](https://ci.appveyor.com/api/projects/status/ja3vf8fp1ros6q4t/branch/master?svg=true)](https://ci.appveyor.com/project/nover/rapidcore/branch/master)

- [![nuget version][core-nuget-image]][core-nuget-url] RapidCore ([see docs](/packages#rapidcore))
- [![nuget version][google-cloud-nuget-image]][google-cloud-nuget-url] RapidCore.GoogleCloud ([see docs](/packages#rapidcoregooglecloud))
- [![nuget version][mongo-nuget-image]][mongo-nuget-url] RapidCore.Mongo ([see docs](/packages#rapidcoremongo))
- [![nuget version][postgres-nuget-image]][postgres-nuget-url] RapidCore.PostgreSql ([see docs](/packages#rapidcorepostgresql))
- [![nuget version][redis-nuget-image]][redis-nuget-url] RapidCore.Redis ([see docs](/packages#rapidcoreredis))
- [![nuget version][sqlserver-nuget-image]][sqlserver-nuget-url] RapidCore.SqlServer ([see docs](/packages#rapidcoresqlserver))
- [![nuget version][xunit-nuget-image]][xunit-nuget-url] RapidCore.Xunit ([see docs](/packages#rapidcorexunit))

[core-nuget-image]: https://img.shields.io/nuget/v/RapidCore.svg
[core-nuget-url]: https://www.nuget.org/packages/RapidCore

[google-cloud-nuget-image]: https://img.shields.io/nuget/v/RapidCore.GoogleCloud.svg
[google-cloud-nuget-url]: https://www.nuget.org/packages/RapidCore.GoogleCloud

[mongo-nuget-image]: https://img.shields.io/nuget/v/RapidCore.Mongo.svg
[mongo-nuget-url]: https://www.nuget.org/packages/RapidCore.Mongo

[postgres-nuget-image]: https://img.shields.io/nuget/v/RapidCore.PostgreSql.svg
[postgres-nuget-url]: https://www.nuget.org/packages/RapidCore.PostgreSql

[redis-nuget-image]: https://img.shields.io/nuget/v/RapidCore.Redis.svg
[redis-nuget-url]: https://www.nuget.org/packages/RapidCore.Redis

[sqlserver-nuget-image]: https://img.shields.io/nuget/v/RapidCore.SqlServer.svg
[sqlserver-nuget-url]: https://www.nuget.org/packages/RapidCore.SqlServer

[xunit-nuget-image]: https://img.shields.io/nuget/v/RapidCore.Xunit.svg
[xunit-nuget-url]: https://www.nuget.org/packages/RapidCore.Xunit

## Resources

- [Source code](https://github.com/rapidcore/rapidcore) - See and contribute to the source code
- [Report an issue](https://github.com/rapidcore/rapidcore/issues)
- [Request a feature](https://github.com/rapidcore/rapidcore/issues)
- [Release notes](https://github.com/rapidcore/rapidcore/releases)

## Contributing

When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

Please note we have a code of conduct, please follow it in all your interactions with the project.

### Pull Request Process

1. Ensure the code quality
2. Update related docs with related changes.
3. Request the reviewer to merge it for you.

### Code Quality

- The code should be maintainable, readable and as simple as possible.
- The code should be testable and mockable (unit tests should be easy to control).
- Exceptions should be helpful with types that make sense and messages that help you fix/diagnose the problem.

### Versioning

We use [SemVer](http://semver.org/), which means you can depend on RapidCore version `<major>.*`

It also means that while the major version is `0`, the APIs should be considered unstable.