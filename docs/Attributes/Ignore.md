# Ignore

If you are building something that analyzes classes, properties, methods etc. and you wish to give users a way to exclude something, then you can use the `Ignore` attribute.

It can be used on all attribute targets.

```csharp
using RapidCore;

[Ignore]
public class IgnoreMe { }

public class IgnorePartsOfMe
{
    public string ImKewl { get; set; }

    [Ignore]
    public string ImNot { get; set; }

    public int SoNice() { return 15; }

    [Ignore]
    public long NotMe() { return 9; }
}
```
