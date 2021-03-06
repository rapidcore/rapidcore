# Methods

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