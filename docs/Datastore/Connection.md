# High-level Datastore connection

`RapidCore.GoogleCloud.Datastore.DatastoreConnection` is a high-level connection class providing basic methods for working with Entities using the [the ORM](../Orm).

It is not meant to cover every single use-case directly. For the more exotic use-cases, it provides direct access to the low-level connection from Google's SDK.


## Creating an instance

```csharp
using RapidCore.GoogleCloud.Datastore;

var datastoreDb = Google.Cloud.Datastore.V1.DatastoreDb.Create("project-id", "namespace");
var connection = new DatastoreConnection(datastoreDb);
```



## Inserting an entity

```csharp
public class KewlPoco
{
    public string Id { get; set; }
    public string Oh { get; set; }
}

// instantiate the poco
var poco = new KewlPoco { Id = "hot-damn", Oh = "yeah" };

// insert it
await connection.InsertAsync<KewlPoco>(poco);
```


## Updating an entity

```csharp
public class KewlPoco
{
    public string Id { get; set; }
    public string Oh { get; set; }
    public int IntOfDoom { get; set; }
}

// load the document
var poco = ...load entity...;

// make the change
poco.IntOfDoom = 666;

// update it
await connection.UpdateAsync<KewlPoco>(poco);
```


## Loading a single entity

```csharp
public class KewlPoco
{
    public string Id { get; set; }
    public string Oh { get; set; }
}

// load the first document where Oh = yeah
var thePoco = await connection.GetByIdOrDefault<KewlPoco>("theId");

if (thePoco != default(KewlPoco))
{
    Console.WriteLine("Yay, we found the entity");
}
else
{
    Console.WriteLine("Awwww... no entities matched :'(");
}
```


## Deleting an entity

```csharp
public class KewlPoco
{
    public string Id { get; set; }
    public string Oh { get; set; }
}

await connection.DeleteAsync<KewlPoco>("theId");
```

## Upserting an entity

```csharp
public class KewlPoco
{
    public string Id { get; set; }
    public string Oh { get; set; }
}

// instantiate the poco
var poco = new KewlPoco { Id = "hot-damn", Oh = "yeah" };

// insert it
await connection.UpsertAsync<KewlPoco>(poco);
```
