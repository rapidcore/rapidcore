# ORM

`RapidCore.GoogleCloud.Datastore.DatastoreOrm` implements a fairly simple [ORM](https://en.wikipedia.org/wiki/Object-relational_mapping) for [Google Cloud Datastore](https://cloud.google.com/datastore/) that works with [POCOs](https://en.wikipedia.org/wiki/Plain_old_CLR_object).

Note that due to design choices in Datastore, the following datatypes are not supported by this ORM:

- sbyte
- ushort
- uint
- ulong

## TL;DR;

- Works with POCOs - no classes or interfaces required
- Figures out which property is the ID based on property with `[PrimaryKey]` or one of the following names (case-insensitive)
    - id
    - identifier
    - primarykey
    - primary_key
- All POCOs **must have exactly 1 ID**
- **You** must set the ID
- Indexes are **opt-in** (unlike raw Datastore), just add `[Index]` to your property
- Kind defaults to the name of the POCO, but can be overridden with `[Kind(..)]`
- Value names default to the name of the property, but can be overridden with `[Name(..)]`
- This ORM does not make references to other entities


## Usage example

This shows how to define, insert and load a very simple POCO.

```csharp
using RapidCore.GoogleCloud.Datastore;

public class SimpleThingToStoreInDatastore
{
    public string Id { get; set; }
    public int AmountOfAwesome { get; set; }
}

// first, we need a connection to Datastore
var datastoreDb = Google.Cloud.Datastore.V1.DatastoreDb.Create("project-id", "namespace");

// the we create the ORM
var orm = new DatastoreOrm(datastoreDb);

// and a POCO instance
var thePoco = new SimpleThingToStoreInDatastore { Id = "yay", AmountOfAwesome = 3000 };

//
// insert the POCO
//
var datastoreEntity = orm.PocoToEntity(thePoco);
await datastoreDb.InsertAsync(datastoreEntity);

//
// load it
//
var kind = orm.GetKind(typeof(SimpleThingToStoreInDatastore));
var key = orm.GetKey(kind, "yay");
var loadedEntity = await datastoreDb.LookupAsync(key);
var loadedPoco = orm.EntityToPoco<SimpleThingToStoreInDatastore>(loadedEntity);
```

## Kind

All entities must be stored in a kind. To with keeping track of the kinds for your POCOs, the ORM has a method for getting the kind for a given type. It will take the `[Kind(..)]` attribute into account.

```csharp
using RapidCore.GoogleCloud.Datastore;

public class TripleA { ... }

orm.GetKind(typeof(TripleA)); // => "TripleA"

[Kind("AllMyBees")]
public class DoubleB { ... }

orm.GetKind(typeof(DoubleB)); // => "AllMyBees"
```

## IDs / Keys

All POCOs must have exactly 1 ID!

To provide flexibility and easy-of-use, the ORM will automagically find an appropriate property to use as the ID or primary-key.

### Properties

To be used as an ID, a property must either have the `[PrimaryKey]` attribute or one of the following names (case-insensitive):

- id
- identifier
- primarykey
- primary_key

The following will all work.

```csharp
using RapidCore.GoogleCloud.Datastore;

public class A
{
    [PrimaryKey]
    public string YoThisIdMyId { get; set; }
}

public class B
{
    public short Id { get; set; }
}

public class C
{
    public int Identifier { get; set; }
}

public class D
{
    public long PrimaryKey { get; set; }
}

public class E
{
    public Guid Primary_Key { get; set; }
}
```

### ID types

Datastore itself only supports 2 types:

  - long
  - string

The ORM, however, supports:

  - short
  - int
  - long
  - string
  - Guid


## Value names

The name of the POCO property will be used by default as the name of the value in the entity. You can however override using the `[Name(..)]` attribute.

```csharp
using RapidCore.GoogleCloud.Datastore;

public class Poco
{
    public string SingleA { get; set; }

    [Name("MoreBeez")]
    public string DoubleB { get; set; }
}
```

## Indexes

In raw Datastore all values are indexed. This will however incur a higher cost than required, as you typically do not need ALL values to be indexed.

In this ORM you will have to **opt-in** to having a value indexed. You do this with the `[Index]` attribute.

```csharp
using RapidCore.GoogleCloud.Datastore;

public class Ocop
{
    public string NotIndexed { get; set; }

    [Index]
    public string TotallyIndexed { get; set; }
}
```

## Ignoring properties

If your POCO contains a property that you do not want to persist, then simply use the `[Ignore]` attribute on it.

```csharp
public class Kewl
{
    [RapidCore.Ignore]
    public int CalculatedValue => 34 * 2;
}
```
