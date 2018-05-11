# Attributes

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
