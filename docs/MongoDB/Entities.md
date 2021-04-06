# Entities

RapidCore contains the `RapidCore.Mongo.EntityAttribute` attribute class, which is required by the `MongoManager`. It is however **optional** when using the `MongoDbConnection`.


The attribute currently supports:

- defining the name of the collection to use


## Collection name

The code allows you to pass in the name of the collection to work on in every single call, but this can quickly become tedious. If you do not provide the collection name, the code will do the following:

```text
// pseudo-code showing how we find the collection name automatically
// TDocument is the generic type provided to the method being called
if (TDocument is marked with Entity attribute)
{
    if (CollectionName is set on the Entity attribute)
    {
        return entityAttribute.CollectionName;
    }

    return typeof(TDocument).Name;
}
```


The example below, will result in work being done on the `the_kewl_stuff` collection, if the collection name is not given specifically to calls through the RapidCore ORM wrapper (primarily the [MongoDbConnection](../RapidCore.Mongo.MongoDbConnection)).

```csharp
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyAwesomeStuff
{
    [Entity(CollectionName = "the_kewl_stuff")]
    public class KewlEntity
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        
        public int Reference { get; set; }
    }
}
```
