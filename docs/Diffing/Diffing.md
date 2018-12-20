# Diff object instances

Writing code to find the difference in values between two object instances is tedius at best. The `RapidCore.Diffing.StateChangeFinder` automates this process without deciding what to do with the differences found - that part is up to you.

It is built on top of the [InstanceTraverser](../../Reflection/InstanceTraverser).

It..

- finds differences in **values** at the top level
- looks at **fields** and **properties**
- includes private, protected, internal and public members
- follows references to look for **changes in values** (it does not consider the reference itself)
- looks for changes in lists
- looks for changes in dictionaries
- can handle all permutations of the given states being null
- prevents overflow exception by using a [max depth](#max-depth) (which you can set to something appropriate for your case - **default is 10**)
- returns an instance of `StateChanges` which provides you with enough information to to additional processing of the diff ([AuditDiffer does this](../../Audit/Audit))

#### Max depth

If the `StateChangeFinder` reaches the max depth and there is another level to inspect, it does not actually blow up. Instead it skips the next level making it possible for you to continue working with the diff that has already been collected.

The idea behind this, is that either your max depth is too low or we have hit a cyclic reference. If an exception was thrown, we would not be able to continue finding differences and you would end up with an incomplete change set - which would not be good in an audit log context.

#### Examples

- [How to Get diff of two objects](../Examples#how-to-get-diff-of-two-objects)
