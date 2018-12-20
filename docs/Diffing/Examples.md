# Examples

## How to Get diff of two objects

```csharp
class SomeThing
{
    public string Name { get; set; }
    private int Code { get; set; }
    protected OtherThing Other { get; set; }

    public SomeThing SetCode(int value)
    {
        Code = value;
        return this;
    }

    public SomeThing SetOther(OtherThing value)
    {
        Other = value;
        return this;
    }
}

class OtherThing
{
    public string Holy { get; set; }
}

using RapidCore.Diffing;
class SomeController
{
    public void DoSomething()
    {
        var oldState = new SomeThing
        {
            Name = "Chicken"
        }
        .SetCode(666)
        .SetOther(new OtherThing { Holy = "moly" });

        var newState = new SomeThing
        {
            Name = "Cow"
        }
        .SetCode(999)
        .SetOther(new OtherThing { Holy = "cow" });


        var stateChangeFinder = new StateChangeFinder();
        stateChangeFinder.MaxDepth = 123; // defaults to 10

        var changes = stateChangeFinder.GetChanges(oldState, newState);

        foreach (var change in changes)
        {
            Console.WriteLine($"{change.Breadcrumb}: \"{change.OldValue}\" => \"{change.NewValue}\"");
        }
    }
}
```

The above code would print:

```
Name: "Chicken" => "Cow"
Code: "666" => "999"
Other.Holy: "moly" => "cow"
```