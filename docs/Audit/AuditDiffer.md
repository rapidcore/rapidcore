# AuditDiffer

The `AuditDiffer` is an object differ, specifically built for generating diffs that can go into an audit log.

In list form it...

- will find changes made to `public` fields and properties
- allows you to exclude certain fields or properties using the `AuditAttribute`
- allows you to "mask" certain fields or properties using the `AuditAttribute` to set an [IAuditValueMasker](../ValueMasking)

It is built on top of the context agnostic [RapidCore.Diffing.StateChangeFinder](../../Diffing/StateChangeFinder).

Note that it uses [IRapidContainerAdapter](../../DependencyInjection/IRapidContainerAdapter) to create `IAuditValueMasker` instances.

```csharp
using RapidCore.Audit;

class SomeThing
{
    public string Name { get; set; }

    [Audit(ValueMasker=typeof(PasswordAuditValueMasker))]
    public string Password { get; set; }

    [Audit(Include=false)]
    public string NotReallyRelevantToTheLog { get; set; }
}

class SomeController
{
    private readonly AuditDiffer auditDiffer;

    public void AaaaaandAction()
    {
        var oldState = new SomeThing { Name = "T-Rex", Password = "NunYa", NotReallyRelevantToTheLog = "one" };
        var newState = new SomeThing { Name = "Generic pop star", Password = "WhoGivesAPoop", NotReallyRelevantToTheLog = "two" };

        var auditDiff = auditDiffer.GetAuditReadyDiff(oldState, newState);

        // in json form, the audit diff would now be something like
        // {
        //   "changes": [
        //     {
        //       "Breadcrumb": "Name",
        //       "OldValue": "T-Rex",
        //       "NewValue": "Generic pop star"
        //     },
        //     {
        //       "Breadcrumb": "Password",
        //       "OldValue: "******",
        //       "NewValue": "******"
        //     }
        //   ]
        // }
    }
}
```
