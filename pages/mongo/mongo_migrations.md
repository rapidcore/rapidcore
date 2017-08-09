---
title: Mongo Migrations
tags: [migration, getting_started]
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

This guide assumes that you are on Linux.


## 1. Create .Net Core project

We will create a simple web API using standard .Net Core.

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

Our "business" logic will be a list of quotes. We will represent those with the `Quote` class in the root namespace.

```csharp
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace rapidcore_migrations
{
    public class Quote
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        public string Who { get; set; }

        public string Said { get; set; }
    }
}
```


## 3. Add migration for db seeding

We will add a migration that initializes the system with a pre-defined set of quotes.

First we create a new namespace for our migrations, so they are easy to keep track of.

```shell
$ mkdir ~/rapidcore-migrations/Migrations
```

Then we create our first migration by adding a class in `Migrations` called `Migration_20170808125300_Initial_seeding`. The name includes a timestamp, as we need to ensure that migrations are always run in the same order as they were created. The system sorts the migrations alphabetically by name, so including the timestamp is an excellent pattern. It also includes a short description (`Initial_seeding`) to make it easier to identify migrations once you have a bunch of them.

```csharp
using MongoDB.Driver;
using RapidCore.Mongo.Migration;

namespace rapidcore_migrations.Migrations
{
    public class Migration_20170808125300_Initial_seeding : MigrationBase
    {
        public override void ConfigureUpgrade(IMigrationBuilder builder)
        {
            builder.Step("Real russians and proverbs", () =>
            {
                base.Context
                    .ConnectionProvider
                    .Default()
                    .GetCollection<Quote>("Quote")
                    .InsertOne(new Quote { Who = "Red", Said = "Real russians don't have proverbs - only Vodka and misery."});
            });

            builder.Step("It's tough to be drunk", () =>
            {
                base.Context
                    .ConnectionProvider
                    .Default()
                    .GetCollection<Quote>("Quote")
                    .InsertOne(new Quote { Who = "Tyrion Lannister", Said = "It's not easy being drunk all the time. If it were easy, everyone would do it." });
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

We need to add a way to initiate running the migrations. For this guide we will simply run new migrations on every application boot. Open up `Program` and ensure that `Main` looks similar to the following.

```csharp
using RapidCore.Mongo.Migration; // <--- new
using RapidCore.Threading; // <--- new

public static void Main(string[] args)
{
    var host = new WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseIISIntegration()
        .UseStartup<Startup>()
        .Build();

    host.Services.GetService(typeof(MigrationRunner)).UpgradeAsync().AwaitSync(); // <--- new

    host.Run();
}
```

Now we need to ensure that the container can actually fulfill the request for a `MigrationRunner`, so let us move over to `Startup` so we can register the stuff we need. `ConfigureServices` should look something like this.

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
