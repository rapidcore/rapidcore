# Nullable

These are extension methods for working with `Nullable<T>` in a reflection context.

## Is it nullable?

```csharp
using RapidCore.Reflection;

typeof(int).IsNullable(); // => false
typeof(int?).IsNullable(); // => true
typeof(string).IsNullable(); // => false .. a string can be null, but is not a Nullable<T>
```

## Getting the underlying type

What is the `T` in `Nullable<T>`?

```csharp
using RapidCore.Reflection;

typeof(int?).GetUnderlyingNullableType(); // => int
typeof(DateTime?).GetUnderlyingNullableType(); // => datetime
```

## Just get me the type

In some cases you do not care if a type is nullable or not, you just want to know what the "actual" type is.

```csharp
using RapidCore.Reflection;

typeof(int?).GetTypeOrUnderlyingNullableType(); // => int
typeof(DateTime?).GetTypeOrUnderlyingNullableType(); // => datetime
typeof(string).GetTypeOrUnderlyingNullableType(); // => string
```
