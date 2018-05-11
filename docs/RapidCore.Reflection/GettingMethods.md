# Getting methods

These methods can be used to get references to methods on an object.

## Get method recursively

The `System.Type.GetMethodRecursively` extension method, tries to find the method you are asking for, anywhere in the type hierarchy (this is the recursive part of the name).

Note that it tries to find an exact method and not just all methods with a given name.

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
