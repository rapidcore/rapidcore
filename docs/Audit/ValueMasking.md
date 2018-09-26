# Value masking

Value masking is best known from creditcards where you are not allowed to see the full card number, but only the first 6 and the last 4 digits - i.e. `1234-5678-9012-3456` becomes `1234-56**-****-3456`.

This same approach can be applied to other things that you do not want to go directly into the audit log.

The [AuditDiffer](../AuditDiffer) allows you to control the representation of a value from a field, by telling it to use a specific implementation of `IAuditValueMasker` via the `AuditAttribute`.

It could be that you are writing a log entry about the changes on a user profile. This set of changes happens to change the password. We are interested in having the log show that the password was changed, but we do not want it to contain any clues as to what the password was or is being changed to. In this case you can use the built-in `PasswordAuditValueMasker`, which **always returns** `******`. That way you can see in the diff that the password was changed, but the value will be presented as `******` for both the previous and new value.

I.e. the diff written as JSON would look like this:

```json
{
  "changes": [
    {
      "Breadcrumb": "Password",
      "OldValue: "******",
      "NewValue": "******"
    }
  ]
}
```
