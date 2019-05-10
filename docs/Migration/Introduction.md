# Introduction

When we say `migration` we mean _code that moves the environment from state `A` to state `B`_.

You would typically add this code while implementing a new version of your system and then run it during the deployment process.

`RapidCore.Migration` provides a storage agnostic framework for doing this. The framework itself is able to find migration code that has not been run yet, run it and record some meta-data around that run.

The key part is the `MigrationRunner`. This is the thing you will instantiate and which will do the actual orchestration of the migrations. Because we have tried to make it as agnostic as possible, it takes in a few interfaces - for which default implementations are available.

Each migration is a simple class that implements the `IMigration` interface, **but** there is an abstract implementation available that will help you a bit. It is called `MigrationBase`.

The framework currently supports upgrading and has been prepared for downgrading, but we still have not figured out exactly how we want the user experience of downgrading to work. If you have any input, [help us out](https://github.com/rapidcore/rapidcore/issues/30).


## IRapidContainerAdapter

This is used to get instances of migration classes ([see the docs](../../DependencyInjection/IRapidContainerAdapter)).


## IMigrationEnvironment

This is not used by the framework itself, but is available to you in your migrations allowing you to act appropriately according to which environment your code is running in. The concept is very similar to `Microsoft.AspNetCore.Hosting.IHostingEnvironment`, except that we put the "test methods" (e.g. `IsDevelopment()`) directly on the interface rather than making it an extension method, as this allows you to mock it if needed.

We chose this abstraction instead of relying on the ASP.NET Core MVC `IHostingEnvironment` as your migrations might be needed in a non-web context - or you simply do not use ASP.NET Core MVC.

There is a default implementation called `MigrationEnvironment`, which take the environment name as input.


## IDistributedAppLockProvider

This is used to ensure that your migrations are not run by multiple instances at the same time ([see the docs](../../RapidCore.Locking/DistributedAppLock)).

By default `MigrationRunner` defines the **name of the lock** as `RapidCoreMigrations`, but you can easily change this by inheriting `MigrationRunner` and overriding the `GetLockName` method. This will be necessary if you have a bunch of services that all have their own data silos and migrations - otherwise they will block each other for no reason.


## IMigrationContextFactory

This is in charge of creating instances of `IMigrationContext` which are provided to each `IMigration` in order to make various helpers available - e.g. the `IMigrationEnvironment` and `IRapidContainerAdapter`. You are in charge of implementing it, as it also contains the storage interface (`IMigrationStorage`).

This is already implemented in the migration stacks provided by RapidCore - e.g. `RapidCore.Mongo.Migration.MongoMigrationContextFactory`.


## IMigrationFinder

The `IMigrationFinder` is in charge of actually finding migrations. RapidCore provides a reflection based implementation called `RapidCore.Migration.ReflectionMigrationFinder`.


### ReflectionMigrationFinder

This uses reflection to find all classes in the specified assemblies that implement the `IMigration` interface. It then sorts them alphabetically by their name, which is why we recommend using a naming pattern like `Migration_<date>_<time>_<short_description>` e.g. `Migration_20190506_160500_initial_seeds`.


## IMigrationStorage

`IMigrationStorage` is in charge of all storage related actions. You should only have to implement this if you use a storage system that RapidCore does not have a specific package for.
