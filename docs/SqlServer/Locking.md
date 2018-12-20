# SqlServer Distributed Application Lock

The `SqlServerDistributedAppLockProvider` is a concrete Distributed Application lock implementation using SqlServer 2008+ as the locking backend. Under the hood it is using the stored procedure [`sp_getapplock`](https://docs.microsoft.com/en-us/sql/relational-databases/system-stored-procedures/sp-getapplock-transact-sql?view=sql-server-2017)

As described in the [Distributed app locks](../Locking/DistributedAppLock.md) section your code should take a dependency on the `IDistributedAppLockProvider` and use the `Acquire` or `AcquireAsync` methods to grab a lock.

## Basic container registration

The SQL provider takes in a `Func<IDbConnection>` factory which must provide an _open_ database connection back. This can be registered in your IoC container of choice:

```csharp
using System;
using System.Data;
using System.Data.SqlClient;
using RapidCore.Locking;
using RapidCore.SqlServer.Locking;

public class Startup
{
    public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
    {
        Environment = hostingEnvironment;
        Configuration = configuration;
    }

    public IHostingEnvironment Environment { get; }
    public IConfiguration Configuration { get; }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IDistributedAppLockProvider>(
            new SqlServerDistributedAppLockProvider(() => {
                var connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
        } ));
    }
}
```

It is safe to register the `provider` as a singleton as it hands out new lock instances for each acquire.

It is very important that the `IDbConnection` instance returned by the connection factory `Func` is being opened (by calling the `Open()` method) as the underlying tooling will otherwise open and close the connection immediately, which renders the lock useless.

Also please note that the default configuration of the `SqlServerDistributedAppLockProvider` will manage the connection for you, thus when the lock is disposed, so will the database connection. This is to ensure that connections do not leak.

### Usage with Entity Framework

Given that the lock provider operates on the lowest level of database connections, namely the `IDbConnection` it supports using your current connection from entity framework - everything but the service registration has been removed for brevity. As EF will manage the connection state you have to opt out of the db connection state management...

```csharp
// Ensure that you have registered your DbContext before this Add
services.AddSingleton<IDistributedAppLockProvider>(container => {
    var db = container.GetService<YourDbContext>();
    return new SqlServerDistributedAppLockProvider(() => {
        var connection = db.Database.GetDbConnection();
        connection.Open();
        return connection;
    }, new SqlServerDistributedAppLockConfig
    {
        DisposeDbConnection = false
    });
});
```