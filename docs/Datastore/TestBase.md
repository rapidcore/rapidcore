# Test base

When you want to write any kind of test using an actual Datastore (e.g. integration tests), then you can use `RapidCore.GoogleCloud.Testing.DatastoreConnectedTestBase` as the base class.

It provides helper methods for common tasks, such as:

- connecting and ensuring that your test class gets its own namespace
- ensuring that a given Kind is empty
- creating keys
- inserting entities
- getting a list of all entities from a Kind

Note that this base class only uses the low-level `DatastoreDb` from Google. It does not depend on or use the high-level connection or the ORM from RapidCore.

You can control the connection parameters via environment variables:

- `RAPIDCORE_DATASTORE_PROJECT_ID` defaults to `rapidcore-local`
- `RAPIDCORE_DATASTORE_URL` defaults to `localhost:8081` (assuming that you are running [the emulator](https://cloud.google.com/datastore/docs/tools/datastore-emulator))

If you need even further control, it also allows you to override the methods it uses to determine project-id, namespace etc.

```csharp
using RapidCore.GoogleCloud.Testing;

public class DoingStuffWithDatastore : DatastoreConnectedTestBase
{
    [Fact]
    public void Inserting()
    {
        EnsureEmptyKind("SoKind");

        Insert(new Entity
        {
            Key = GetKey("SoKind", 1),
            ["String"] = "one",
            ["X"] = 3
        });
        
        Insert(new Entity
        {
            Key = GetKey("SoKind", 2),
            ["String"] = "two",
            ["X"] = 3
        });

        var all = GetAll("SoKind");

        Assert.Equal(2, all.Count);
    }
}
```
