# Examples

## Attributes

These methods exist in both `..(Type attribute)` and `..<TAttribute>()` forms. The generic versions are useful when you know at compile time what you are working with. The non-generic versions are useful when you do not have this knowledge.

```csharp
using System.Reflection;
using RapidCore.Reflection;

MemberInfo memberInfo = ...;

// does this member have my attribute?
if (memberInfo.HasAttribute<MyAttribute>())
{
    // yes it does - act accordingly
}

// do something with the data from attributes
foreach (MyAttribute attr in memberInfo.GetSpecificAttribute<MyAttribute>())
{
    Console.WriteLine(attr.SomethingKewl);
}
```

## Get method recursively

The `System.Type.GetMethodRecursively` extension method, tries to find the method you are asking for, anywhere in the type hierarchy (this is the recursive part of the name).

Note that it **tries to find an exact method** and not just all methods with a given name.

```csharp
using System.Reflection;
using RapidCore.Reflection;

class ZseParent
{
    public void KewlMethod(string str) { }
}

class ZseChild : ZseParent
{
    public void KewlMethod(int) { }
}


var instance = new ZseChild();

MethodInfo method = instance.GetMethodRecursively(
    "KewlMethod",
    typeof(string)
);
// method now points to ZseParent.KewlMethod(string)
```


## Invoke method recursively

The extension method `System.Object.InvokeMethodRecursively` takes a method name and the parameters to call that method with. The **recursively** part of the name, means _finds the method anywhere in the object hierarchy_ - i.e. if the method is actually defined in a parent class, it is still found and invoked.

```csharp
using RapidCore.Reflection;

class TheParent
{
    public string SaySomethingToMe(string who, string what)
    {
        return $"Yo {who}! {what}";
    }
}

class TheChild : TheParent
{
}


var child = new TheChild();

var whatWasSaid = (string)child.InvokeMethodRecursively(
    "SaySomethingToMe", // the name of the method
    "Sandy", // first param
    "Does he have a car?" // second param
);
// => "Yo Sandy! Does he have a car?
```


## Invoke generic method recursively

This is the exact same concept as above, but with support for calling a generic method.

```csharp
using RapidCore.Reflection;

class TheGenericParent
{
    public string SaySomethingToMe<T1, T2>(string who, T1 one, T2 two)
    {
        return $"Yo {who}! {one} {two}";
    }
}

class TheGenericChild : TheParent
{
}


var child = new TheGenericChild();

var whatWasSaid = (string)child.InvokeGenericMethodRecursively(
    "SaySomethingToMe", // the name of the method
    new Type[] { typeof(int), typeof(long) }, // the type arguments - i.e. <T1, T2>
    "Sandy", // first param
    111, // second param
    222L // third param
);
// => "Yo Sandy! 111 222"
```

## Is it nullable?

```csharp
using RapidCore.Reflection;

typeof(int).IsNullable(); // => false
typeof(int?).IsNullable(); // => true
typeof(string).IsNullable(); // => false .. a string can be null, but is not a Nullable<T>
```

## Getting the underlying type

What is the `T` in `Nullable<T>`?

```csharp
using RapidCore.Reflection;

typeof(int?).GetUnderlyingNullableType(); // => int
typeof(DateTime?).GetUnderlyingNullableType(); // => datetime
```

## Just get me the type

In some cases you do not care if a type is nullable or not, you just want to know what the "actual" type is.

```csharp
using RapidCore.Reflection;

typeof(int?).GetTypeOrUnderlyingNullableType(); // => int
typeof(DateTime?).GetTypeOrUnderlyingNullableType(); // => datetime
typeof(string).GetTypeOrUnderlyingNullableType(); // => string
```

## Get property recursively

The `System.Type.GetMethodRecursively` extension method, tries to find the property you are asking for, anywhere in the type hierarchy (this is the recursive part of the name).


```csharp
using System.Reflection;
using RapidCore.Reflection;

class DasParent
{
    public string TheProp { get; set; }
}

class DasChild : DasParent
{
}


var instance = new DasChild();

PropertyInfo property = instance.GetPropertyRecursively("TheProp");
// property now points to DasParent.TheProp
```


## Invoke property methods

These methods allow you to call a propertie's **getter** and **setter** dynamically. _Recursively_ means "anywhere in the type hierarchy".


```csharp
using RapidCore.Reflection;

class HazMadPropz
{
    public string SuitType { get; set; }
}


var instance = new HazMadPropz();

// set the value
instance.InvokeSetterRecursively("SuitType", "Plastic");

// get the value
var res = instance.InvokeGetterRecursively("SuitType");
// => "Plastic"
```

## Traversing an object instance

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