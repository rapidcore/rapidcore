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
