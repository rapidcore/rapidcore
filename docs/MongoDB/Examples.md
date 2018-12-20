# Examples

## Creating an instance

```csharp
using RapidCore.Mongo;

var client = new MongoDB.Driver.MongoClient("mongodb://localhost:27017");
var connection = new MongoDbConnection(client.GetDatabase("the_name_of_your_db"));
```

## Inserting a document

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

## Updating a document

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

## Loading a single document

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

## Deleting documents

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

## The individual index to properties

```csharp
using RapidCore.Mongo;

[Entity]
public class Entity
{
    [Index]
    public string One { get; set; }

    [Index]
    public string Two { get; set; }
}
```

Created Indexes

```json
{
    "key": {
        "One": 1
    },
    "name": "One_1",
    "background": true,
    "sparse": false
}
,
{
    "key": {
        "Two": 1
    },
    "name": "Two_1",
    "background": true,
    "sparse": false
}
```

## Compound indexes

If we want to support a query using both our properties, we should add a compound index - i.e. an index with both properties. We can even add the compound index in addition to the individual indexes.

```csharp
using RapidCore.Mongo;

[Entity]
public class Entity
{
    [Index]
    [Index("two_and_one", Order = 2)]
    public string One { get; set; }

    [Index]
    [Index("two_and_one", Order = 1)]
    public string Two { get; set; }
}
```

This will create indexes like...

```json
{
    "key": {
        "One": 1
    },
    "name": "One_1"
},
{
    "key": {
        "Two": 1
    },
    "name": "Two_1"
},
{
    "key": {
        "Two": 1,
        "One": 1
    },
    "name": "two_and_one"
}
```

It is optional to specify the order of the properties for a compound index.

## Indexes on nested documents

You can also add indexes on nested documents using the exact same pattern, except that your **nested document does not need** to be marked with `[Entity]`.

```csharp
using RapidCore.Mongo;

[Entity]
public class Entity
{
    [Index]
    public string One { get; set; }

    public Nested Nested { get; set; }
}

public class Nested
{
    [Index]
    public string FromNested { get; set; }
}
```

This will create indexes like...

```json
{
    "key": {
        "One": 1
    },
    "name": "One_1"
},
{
    "key": {
        "Nested.FromNested": 1
    },
    "name": "Nested.FromNested_1"
}
```

## Ensure the indexes are created

The `RapidCore.Mongo.MongoManager` class can help you ensure that the indexes you have so carefully crafted, are actually created in the database.

```csharp
using RapidCore.Mongo;

// instantiate the "low"-level mongo client
var client = new MongoDB.Driver.MongoClient("mongodb://localhost:27017");

// instantiate the manager
var manager = new MongoManager();

// ensure indexes from entities in given assembly and namespace
manager.EnsureIndexes(
    client.GetDatabase("TheNameOfYourDatabase"),
    typeof(YourEntity).GetTypeInfo().Assembly,
    typeof(YourEntity).GetTypeInfo().Namespace
);
```

`EnsureIndexes` will scan the given assembly for classes in the given namespace that have been marked with `[RapidCore.Mongo.Entity]`.

Due to the current state of reflection in .NET Core, you have to provide the assembly to scan.