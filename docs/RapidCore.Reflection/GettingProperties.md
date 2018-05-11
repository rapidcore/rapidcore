# Getting properties

These methods can be used to get references to properties on an object.

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
