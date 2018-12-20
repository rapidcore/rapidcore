# Packages

The library is split into multiple packages (based on external dependencies).

### RapidCore

[![nuget version][nuget-image-core]][nuget-url-core]

- Audit ([see docs](/Audit/Audit))
- Diffing ([see docs](/Diffing/Diffing))
- Globalization ([see docs](/Globalization/DateAndTime))
- Threading ([see docs](/Threading/AsyncAwait))
- Environment Variables ([see docs](/EnvironmentVariables/EnvironmentVariables))
- Generally useful attributes ([see docs](/Attributes/Ignore))
- Distributed app lock ([see docs](/Locking/DistributedAppLock))
- Network ([see docs](/Network/DNS))
- Reflection ([see docs](/Reflection/Attributes))
- Dependency injection ([see docs](/DependencyInjection/IRapidContainerAdapter))

### RapidCore.GoogleCloud

[![nuget version][nuget-image-google-cloud]][nuget-url-google-cloud]

RapidCore classes for [Google Cloud Platform](https://cloud.google.com) that depend on various SDKs.

- Google Cloud Datastore ([see docs](/Datastore/Introduction))

### RapidCore.Mongo

[![nuget version][nuget-image-mongo]][nuget-url-mongo]

RapidCore classes that depend on MongoDB.

- Mongo DB ([see docs](/MongoDB/Connection))

### RapidCore.PostgreSql

[![nuget version][nuget-image-postgres]][nuget-url-postgres]

RapidCore classes that depend on PostgreSql.

- PostgreSql ([see docs](/PostgreSql/PostgreSql))

### RapidCore.Redis

[![nuget version][nuget-image-redis]][nuget-url-redis]

RapidCore classes that depend on Redis.

- Redis ([see docs](/Redis/Redis))

### RapidCore.SqlServer

[![nuget version][nuget-image-sqlserver]][nuget-url-sqlserver]

RapidCore classes that depend on SqlServer.

- SqlServer ([see docs](/SqlServer/Locking))

### RapidCore.Xunit

[![nuget version][nuget-image-xunit]][nuget-url-xunit]

Contains helper stuff for writing xunit tests.

Current features

Provides an `ILoggerFactory` extension for adding an `ILogger` that writes to the xunit output buffer. Simply issue: `LogFactory.AddXunitOutput(output)` passing it the `ITestOutputHelper` instance

- Xunit ([see docs](/Xunit/Xunit))

[nuget-image-core]: https://img.shields.io/nuget/v/RapidCore.svg
[nuget-url-core]: https://www.nuget.org/packages/RapidCore

[nuget-image-google-cloud]: https://img.shields.io/nuget/v/RapidCore.GoogleCloud.svg
[nuget-url-google-cloud]: https://www.nuget.org/packages/RapidCore.GoogleCloud

[nuget-image-mongo]: https://img.shields.io/nuget/v/RapidCore.Mongo.svg
[nuget-url-mongo]: https://www.nuget.org/packages/RapidCore.Mongo

[nuget-image-postgres]: https://img.shields.io/nuget/v/RapidCore.PostgreSql.svg
[nuget-url-postgres]: https://www.nuget.org/packages/RapidCore.PostgreSql

[nuget-image-redis]: https://img.shields.io/nuget/v/RapidCore.Redis.svg
[nuget-url-redis]: https://www.nuget.org/packages/RapidCore.Redis

[nuget-image-sqlserver]: https://img.shields.io/nuget/v/RapidCore.SqlServer.svg
[nuget-url-sqlserver]: https://www.nuget.org/packages/RapidCore.SqlServer

[nuget-image-xunit]: https://img.shields.io/nuget/v/RapidCore.Xunit.svg
[nuget-url-xunit]: https://www.nuget.org/packages/RapidCore.Xunit
