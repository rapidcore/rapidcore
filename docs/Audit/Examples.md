# Examples

## How to Get Audit Diff of two objects

```csharp
using RapidCore.Audit;

class User
{
    public string Name { get; set; }
    public string Address { get; set; }
}

class SomeController
{
    private readonly AuditDiffer auditDiffer;

    public void DiffAction()
    {
        var oldState = new User { Name = "T-Rex", Address = "Address 1" };
        var newState = new User { Name = "Generic pop star", Address = "Address 2" };

        var auditDiff = auditDiffer.GetAuditReadyDiff(oldState, newState);

        Console.WriteLine(auditDiff);
    }
}
```

Result

```json
{
  "changes": [
    {
      "Breadcrumb": "Name",
      "OldValue": "T-Rex",
      "NewValue": "Generic pop star"
    },
    {
      "Breadcrumb": "Address",
      "OldValue": "Address 1",
      "NewValue": "Address 2"
    }
  ]
}
```

## How to use "PasswordAuditValueMasker" to mask password field in audit diff

```csharp
using RapidCore.Audit;

class User
{
    public string Name { get; set; }
    public string Address { get; set; }

    [Audit(ValueMasker=typeof(PasswordAuditValueMasker))]
    public string Password { get; set; }
}

class SomeController
{
    private readonly AuditDiffer auditDiffer;

    public void DiffAction()
    {
        var oldState = new User { Name = "T-Rex", Address = "Address 1", Password = "NunYa" };
        var newState = new User { Name = "Generic pop star", Address = "Address 2", Password = "WhoGivesAPoop" };

        var auditDiff = auditDiffer.GetAuditReadyDiff(oldState, newState);

        Console.WriteLine(auditDiff);
    }
}
```

Result

```json
{
  "changes": [
    {
      "Breadcrumb": "Name",
      "OldValue": "T-Rex",
      "NewValue": "Generic pop star"
    },
    {
      "Breadcrumb": "Address",
      "OldValue": "Address 1",
      "NewValue": "Address 2"
    },
    {
      "Breadcrumb": "Password",
      "OldValue": "******",
      "NewValue": "******"
    }
  ]
}
```

## How to use "Include" attribute to exclude certain properties in audit diff

```csharp
using RapidCore.Audit;

class User
{
    public string Name { get; set; }
    public string Address { get; set; }

    [Audit(Include=false)]
    public string NotReallyRelevantToTheLog { get; set; }
}

class SomeController
{
    private readonly AuditDiffer auditDiffer;

    public void DiffAction()
    {
        var oldState = new User { Name = "T-Rex", Address = "Address 1", NotReallyRelevantToTheLog = "one" };
        var newState = new User { Name = "Generic pop star", Address = "Address 2", NotReallyRelevantToTheLog = "rwo" };

        var auditDiff = auditDiffer.GetAuditReadyDiff(oldState, newState);

        Console.WriteLine(auditDiff);
    }
}
```

Result

```json
{
  "changes": [
    {
      "Breadcrumb": "Name",
      "OldValue": "T-Rex",
      "NewValue": "Generic pop star"
    },
    {
      "Breadcrumb": "Address",
      "OldValue": "Address 1",
      "NewValue": "Address 2"
    }
  ]
}
```