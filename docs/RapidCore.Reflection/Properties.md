# Properties

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
