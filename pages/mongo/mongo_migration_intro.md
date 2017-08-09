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

## Migrations and MongoDB - are you drunk?

Many will argue that you do not need migrations when you use MongoDB as Mongo itself is schema-less.

As we see it, your data is still following a schema - it is merely enforced by the application rather than the database. Either you will have to support multiple versions of your "schema" in your application or you can write code that transforms the data from your previous schema to the new schema.

A migration should also take of the data itself and not only be about the structure of the data.
