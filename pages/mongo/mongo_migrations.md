---
title: Mongo Migrations
tags: [migration]
keywords:
summary: "Mongo Migrations is here."
sidebar: mongo_sidebar
permalink: mongo_migrations.html
folder: mongo
---

Getting started with Mongo and data migrations
==============================================

Beware: This functionality is still very new and subject to change.

When we say `migration` we mean _code that moves the environment from state `A` to state `B`_.

You would typically add this code while implementing a new version of your system and then run it during the deployment process.

We will go through the following steps:

1. Create .Net Core project
2. Add business logic
3. Add migration for db seeding
4. Setup `MigrationRunner` so we can run stuff


## 1. Create .Net Core project

This guide assumes that you are on linux.

```shell
$ cd ~
$ mkdir rapidcore-migrations
$ cd rapidcore-migrations
$ dotnet new webapi
$ dotnet add package RapidCore.Mongo
$ dotnet restore
```

You should now have a directory in your home folder containing a basic web api with a `Program.cs`, `Startup.cs` and a bunch of other files.


## 2. Add business logic

Add a class in the root called `Message`.

```csharp
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace rapidcore_migrations
{
    public class Message
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        public string Say { get; set; }
    }
}
```

This will serve our very simple purpose of saying stuff in the db :)


## 3. Add migration for db seeding

```shell
$ mkdir Migrations
```

Add a class in `Migrations` called `Migration_20170808125300`.

```csharp
using MongoDB.Driver;
using RapidCore.Mongo.Migration;
using RapidCore.Mongo.Migration.Internal;

namespace rapidcore_migrations.Migrations
{
    public class Migration_20170808125300 : MigrationBase
    {
        public override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            builder.Step("Say hello", () =>
            {
                Context
                    .ConnectionProvider
                    .Default()
                    .GetCollection<Message>("Message")
                    .InsertOne(new Message { Say = "Hello" });
            });

            builder.Step("Say bye", () =>
            {
                Context
                    .ConnectionProvider
                    .Default()
                    .GetCollection<Message>("Message")
                    .InsertOne(new Message { Say = "Bye" });
            });
        }

        public override void ConfigureDowngrade(IMigrationBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}
```

## 4. Setup `MigrationRunner` so we can run stuff

In the root, create a new class called `Migrator`.

```csharp
using Microsoft.Extensions.Logging;
using RapidCore.Mongo.Migration;
using RapidCore.Threading;

namespace rapidcore_migrations
{
    public class Migrator
    {
        private readonly MigrationRunner runner;

        public Migrator(MigrationRunner runner)
        {
            this.runner = runner;
        }

        public void RunMigrations()
        {
            runner.UpgradeAsync().AwaitSync();
        }
    }
}
```

We need to add a way to initiate running the migrations. For this guide we will simply run new migrations on every application boot. Open up `Program` and ensure that `Main` looks similar to the following.

```csharp
public static void Main(string[] args)
{
    var host = new WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseIISIntegration()
        .UseStartup<Startup>()
        .Build();

    host.Services.GetService(typeof(Migrator)).RunMigrations(); // <--- this is the new stuff

    host.Run();
}
```

Now we need to ensure that the container can actually fulfill the request for a `Migrator`, so let us move over to `Startup` so we can register the stuff we need. `ConfigureServices` should look something like this.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Add framework services.
    services.AddMvc();

    services.AddSingleton<MigrationRunner>(container => {
        return new YoloMigrationRunner(
            container,
            "testing", // <--- your environment name
            "mongodb://localhost:27017", // <--- mongo connection string
            "rapidcore_migrations", // <--- mongo database name
            new NoopDistributedAppLockProvider(),
            typeof(Startup).GetTypeInfo().Assembly
        );
    });
}
```

Also, add the following usings to `Startup`.

```csharp
using System.Reflection;
using RapidCore.Locking;
using RapidCore.Mongo;
```
