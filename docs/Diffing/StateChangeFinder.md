# Diff object instances

Writing code to find the difference in values between two object instances is tedius at best. The `RapidCore.Diffing.StateChangeFinder` automates this process without deciding what to do with the differences found - that part is up to you.

It is built on top of the [InstanceTraverser](../../RapidCore.Reflection/InstanceTraverser).

It..

- finds differences in **values** at the top level
- looks at **fields** and **properties**
- includes private, protected, internal and public members
- follows references to look for **changes in values** (it does not consider the reference itself)
- looks for changes in lists
- looks for changes in dictionaries
- can handle all permutations of the given states being null
- prevents overflow exception by using a [max depth](#max-depth) (which you can set to something appropriate for your case - **default is 10**)
- returns an instance of `StateChanges` which provides you with enough information to to additional processing of the diff ([AuditDiffer does this](../../Audit/AuditDiffer))


## Usage

Using it is quite simple:

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

## Max depth

If the `StateChangeFinder` reaches the max depth and there is another level to inspect, it does not actually blow up. Instead it skips the next level making it possible for you to continue working with the diff that has already been collected.

The idea behind this, is that either your max depth is too low or we have hit a cyclic reference. If an exception was thrown, we would not be able to continue finding differences and you would end up with an incomplete change set - which would not be good in an audit log context.
