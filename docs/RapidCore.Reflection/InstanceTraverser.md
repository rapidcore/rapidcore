# Traversing an object instance

Traversing an instance of an object is useful in many reflection based tasks, but it has a bunch of pitfalls and requires a seemingly infinite amount of tests.

Instead of repeating a bunch of these things every time we build something with reflection, we now have `RapidCore.Reflection.InstanceTraverser`, which can traverse an instance of an object.

It will:

- find all members (constructors, fields, properties and methods regardless of access level) of the instance and tell you about them through an `IInstanceListener` ([StateChangeFinder](../../Diffing/StateChangeFinder) does this)
- not do anything with what it finds except tell you about it
- follow complex values (i.e. recurse into other instances held by the members)
- notify you if it hits the given maximum depth during recursion
- traverse elements in enumerables, including recursing complex values
- traverse dictionaries, including recursing complex values
- ignore backing fields and methods generated by the compiler for auto properties


## Usage

First you create a class implementing `IInstanceListener`. It is basically a bunch of methods that acts as "listeners" - i.e. they are called with a context, `MemberInfo` and for fields and properties, a `Func<object>`  which you can invoke to get the value of the member.

It also has the `OnMaxDepthReached` method, which is called if the maximum recursion depth is reached. You can then handle this however you want. If you want processing to stop, you must throw an exception. Otherwise, the traversal will continue without diggin deeper.

```csharp
class MyListener : RapidCore.Reflection.IInstanceListener
{
    // ...
}

class SomeController
{
    public void DoStuff()
    {
        var traverser = new InstanceTraverser();

        var instance = new InstanceOfSomething();

        var listener = new MyListener();

        traverser.TraverseInstance(instance, maxDepth: 10, listener);

        // now your listener has been called for each member in the
        // entire inheritance tree
    }
}
```