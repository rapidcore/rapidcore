---
title: Mongo Connection
tags: [getting_started]
keywords:
summary: "Mongo connection is here."
sidebar: mydoc_sidebar
permalink: mongo_connection.html
folder: mydoc
---
Connecting to MongoDB
=====================

`RapidCore.Mongo.MongoDbConnection` is a high-level connection class.

Creating an instance
--------------------

```csharp
var client = new MongoDB.Driver.MongoClient("mongodb://localhost:27017");
var connection = new MongoDbConnection(client);
```

Inserting a document
--------------------

Currently, only [upsert](https://docs.mongodb.com/manual/reference/method/db.collection.update/#upsert-option) is supported.

```csharp
public class KewlDocument
{
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreIfDefault]
    public MongoDB.Bson.ObjectId Id { get; set; }
    public string Oh { get; set; }
}

// instantiate a document
var theDoc = new KewlDocument { Oh = "yeah" };

// insert it using the upsert method
await connection.UpsertAsync<KewlDocument>("kewl_docs", theDoc, (filter) => filter.Oh == "yeah");

// The ID has been set automatically
Console.WriteLine($"TheDoc.Id = {theDoc.Id}"); 
```


Updating a document
-------------------

Currently, only [upsert](https://docs.mongodb.com/manual/reference/method/db.collection.update/#upsert-option) is supported.

```csharp
public class KewlDocument
{
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreIfDefault]
    public MongoDB.Bson.ObjectId Id { get; set; }
    public string Oh { get; set; }
    public int IntOfDoom { get; set; }
}

// load the document
var theDoc = ...load document...;

// make the change
theDoc.IntOfDoom = 666;

// update it via the upsert method
await connection.UpsertAsync<KewlDocument>("kewl_docs", theDoc, (filter) => filter.Oh == "yeah");
```

Loading a single document
-------------------------

```csharp
public class KewlDocument
{
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreIfDefault]
    public MongoDB.Bson.ObjectId Id { get; set; }
    public string Oh { get; set; }
}

// load the first document where Oh = yeah
var theDoc = await connection.FirstOrDefaultAsync<KewlDocument>("kewl_docs", (filter) => filter.Oh == "yeah");

if (theDoc != default(KewlDocument))
{
    Console.WriteLine("Yay, we found the document");
}
else
{
    Console.WriteLine("Awwww... no docs matched :'(");
}
```

Deleting documents
------------------

```csharp
public class KewlDocument
{
    [MongoDB.Bson.Serialization.Attributes.BsonIgnoreIfDefault]
    public MongoDB.Bson.ObjectId Id { get; set; }
    public string Oh { get; set; }
}

// delete all documents where Oh begins with a y
await connection.DeleteAsync<KewlDocument>("kewl_docs", (filter) => filter.Oh.StartsWith("y"));
```
