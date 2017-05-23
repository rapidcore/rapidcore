Indexes
=======

Having indexes is crucial to query performance. With `RapidCore.Mongo` you can define your indexes using attributes on your entity POCOs.


The magic of MongoManager
-------------------------

The `RapidCore.Mongo.MongoManager` class can help you ensure that the indexes you have so carefully crafted, are actually created in the database.

```csharp
// instantiate the "low"-level mongo client
var client = new MongoDB.Driver.MongoClient("mongodb://localhost:27017");

// instantiate the manager
var manager = new MongoManager(client);

// ensure indexes from entities in given assembly and namespace
manager.EnsureIndexes(
    client.GetDatabase("TheNameOfYourDatabase"),
    typeof(YourEntity).GetTypeInfo().Assembly,
    typeof(YourEntity).GetTypeInfo().Namespace
);
```

`EnsureIndexes` will scan the given assembly for classes in the given namespace that have been marked with `[RapidCore.Mongo.Entity]`.

Due to the current state of reflection in .NET Core, you have to provide the assembly to scan.

### Note that

- All key indexes are made in **ascending** order.
- All indexes are made in the background
- By default indexes are **not** sparse


The simple case
---------------

In this example we just want to add individual indexes to our properties.

```csharp
[Entity]
public class Entity
{
    [Index]
    public string One { get; set; }

    [Index]
    public string Two { get; set; }
}
```

We can see the indexes created with the following mongo javascript

```javascript
db.getCollection('SimpleIndexes').getIndexes().forEach(function(index) { printjson(index); });
```

In this case we will get something like...

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

Because we did not provide a name for the indexes, Mongo just chose some for us.


Compound indexes
----------------

If we want to support a query using both our properties, we should add a compound index - i.e. an index with both properties. We can even add the compound index in addition to the individual indexes.

```csharp
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


Indexes on nested documents
---------------------------

You can also add indexes on nested documents using the exact same pattern, except that your **nested document does not need** to be marked with `[Entity]`.

```csharp
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
