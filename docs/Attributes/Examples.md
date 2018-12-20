# Examples

## How ignore attributes

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