# Audit

Audit is a tool to get a diff of two object of same type. The diff can be used to store as an audit log.
Often an audit log contains information about critical actions like chang on some entry. The diff contains before and after value of every changed properties.

The `AuditDiffer` is an object differ, specifically built for generating diffs that can go into an audit log.

In list form it...

- will find changes made to `public` fields and properties
- allows you to exclude certain fields or properties using the `AuditAttribute`
- allows you to "mask" certain fields or properties using the `AuditAttribute` to set an [IAuditValueMasker](../ValueMasker)

It is built on top of the context agnostic [RapidCore.Diffing.StateChangeFinder](../../Diffing/Diffing).

Note that it uses [IRapidContainerAdapter](../../DependencyInjection/IRapidContainerAdapter) to create `IAuditValueMasker` instances.

You can use some attributes to customize the output from diff.

#### Attributes

- [ValueMasker](../ValueMasker)
- [Include](../Examples#how-to-use-include-attribute-to-exclude-certain-properties-in-audit-diff)

#### Examples

- [How to Get Audit Diff of two objects](../Examples#how-to-get-audit-diff-of-two-objects)
- [How to use "PasswordAuditValueMasker" to mask password field in audit diff](../Examples#how-to-use-passwordauditvaluemasker-to-mask-password-field-in-audit-diff)
- [How to use "Include" attribute to exclude certain properties in audit diff](../Examples#how-to-use-include-attribute-to-exclude-certain-properties-in-audit-diff)