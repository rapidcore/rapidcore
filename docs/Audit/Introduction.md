# Audit

Often and audit log - i.e. a log containing information about critical actions like changing the settings on something - will have something entries like "user X updated entity Y" that also contains a "diff" of "entity Y" before and after the changes were made.

This diff can be very annoying to write, which is why RapidCore now contains a few things that should alleviate the pain:

- an `AuditDiffer` ([see docs](../AuditDiffer))
- the concept of a `value masker` ([see docs](../ValueMasking))
- `AuditAttribute` (which is used by `AuditDiffer`)
