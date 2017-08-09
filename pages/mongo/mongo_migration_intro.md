---
title: Intro to Mongo Migrations
tags: [migration]
keywords: [migration, intro]
sidebar: mongo_sidebar
permalink: /mongo_migration_intro
folder: mongo
---

When we say `migration` we mean _code that moves the environment from state `A` to state `B`_.

You would typically add this code while implementing a new version of your system and then run it during the deployment process.

The migration stack is centered around `RapidCore.Mongo.Migration.MigrationRunner` and `RapidCore.Mongo.Migration.IMigration`.

The framework currently supports upgrading and has been prepared for downgrading, but we still have not figured out exactly how we want the user experience of downgrading to work. If you have any input, [help us out](https://github.com/rapidcore/issues/issues/14).

## Migrations and MongoDB - are you drunk?

Many will argue that you do not need migrations when you use MongoDB as Mongo itself is schema-less.

As we see it, your data is still following a schema - it is merely enforced by the application rather than the database. Either you will have to support multiple versions of your "schema" in your application or you can write code that transforms the data from your previous schema to the new schema.

A migration should also take of the data itself and not only be about the structure of the data.

## How does it work?

The `MigrationRunner` is "just" an orchestrator that can be hooked in to your dependency injection through `RapidCore.Mongo.Migration.IContainerAdapter` - in fact it only depends on interfaces, so you can replace or extend all parts.

The `IMigrationManager` is responsible for finding migrations to run and storing the state of migrations. Our default implementation (`MigrationManager`) looks for classes implementing the `IMigration` interface. It then filters out the migrations that have already been completed.

We have implemented an abstract base class called `MigrationBase` which provides a framework for splitting the individual migrations into steps that are run and tracked individually, making it possible to continue a migration even though an earlier run failed. This would make sense in cases where it was not a code error that made the migration work - or in cases where your migration fails due to a bug in the code that you then fix and deploy.

## The interfaces and our defaults

### `IContainerAdapter`

Simple adapter to abstract away hard dependencies on specific container implementations. We do however provide an implementation for `System.IServiceProvider` (aka the default container used by .Net Core) called `RapidCore.Mongo.Migration.ServiceProviderContainerAdapter`.

### `IMigrationEnvironment`

Carries useful information about the executing environment. We chose this abstraction instead of relying on the ASP.NET Core MVC `IHostingEnvironment` as your migrations might be needed in a non-web context - or you simply do not use ASP.NET Core MVC.

The default implementation is called `RapidCore.Mongo.Migration.MigrationEnvironment`.

### `IConnectionProvider`

We wanted to support cases where you have multiple databases (and therefore multiple connections) in play in your system. Therefore we added `IConnectionProvider` which in its default implementation (`RapidCore.Mongo.Migration.ConnectionProvider`) is a simple wrapper around a dictionary of named connections.

The migration stack itself will always use the `.Default()` connection.

### `IMigrationManager`

Finds and keeps track of migrations. The default implementation `RapidCore.Mongo.Migration.MigrationManager` uses reflection to find implementations of `IMigration` in a list of given assemblies.

### `IDistributedAppLockProvider`

To ensure that only 1 thread can run migrations at a time, we use the distributed app lock stack from `RapidCore.Locking`.

### `IMigration`

This is what it is all about. Your migration code should implement this interface - or even better - implement our abstract `RapidCore.Mongo.Migration.MigrationBase` class.

It defines the methods run when upgrading or downgrading.
