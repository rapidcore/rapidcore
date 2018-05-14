# Date and time

As `DateTime` (and friends) are structs without interfaces and are often created using static methods like `.Now`, they make testing code using them very tricky - the code is either skipping parts that should be tested or end up being unreliable.

In our opinion, dates and times should always be in [UTC](https://en.wikipedia.org/wiki/Coordinated_Universal_Time) in backend code, as that simplifies the work around them. Input should either be required to be in UTC or should be converted to UTC before they are used. It is however difficult to make tests that show that DateTimes created by a piece of code are indeed UTC.

This is the reason why `RapidCore.Globalization.UtcHelper` exists!

The `UtcHelper` does not replace `DateTime`, it simply provides methods for working with them, that you can actually mock + it provides a very clear **UTC context** - there is no way to get a non-UTC DateTime out of it!

## UtcHelper.Now

This is a replacement for `DateTime.UtcNow`. The `UtcHelper` becomes a point in your code where you can control the output by mocking it.

```csharp
using RapidCore.Globalization;

class ShouldBeTested
{
	private readonly UtcHelper utcHelper;
	private readonly ThirdPartyApi thirdParty;

	public ShouldBeTested(UtcHelper utcHelper, ThirdPartyApi thirdParty)
	{
		this.utcHelper = utcHelper;
		this.thirdParty = thirdParty;
	}

	public void Call3rdParty()
	{
		thirdParty.ThisIsMyTime(utcHelper.Now());
	}
}
```

See how we are now able to write a test that shows that we are calling `thirdParty.ThisIsMyTime` with the correct parameters.

## UtcHelper.ToUtc

You can use `UtcHelper.ToUtc` to ensure that a given timestamp is in UTC.

It accepts a `DateTime` or a `String` and gives you back a `DateTìme` which is ensured to be in UTC.

If the timestamp you give it is already UTC, the `DateTime` you get back will still be in UTC.

```csharp
using RapidCore.Globalization;

var utcHelper = new UtcHelper();

//
// we have a DateTime instance that may
// or may not be UTC
//
var definitelyUtc = utcHelper.ToUtc(dateTimeThatMayOrMayNotBeInUtc);

//
// we have a DateTime in the form of a string
// that may or may not be in UTC
//
// it accepts whatever DateTime.Parse accepts
//
var utcInstance = utcHelper.ToUtc(whateverDateTimeParseAccepts);
```

For more examples [see the unit tests](https://github.com/rapidcore/rapidcore/blob/master/src/core/test-unit/Globalization/UtcHelperTests.cs).
