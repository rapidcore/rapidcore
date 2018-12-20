# Date and time

As `DateTime` (and friends) are structs without interfaces and are often created using static methods like `.Now`, they make testing code using them very tricky - the code is either skipping parts that should be tested or end up being unreliable.

In our opinion, dates and times should always be in [UTC](https://en.wikipedia.org/wiki/Coordinated_Universal_Time) in backend code, as that simplifies the work around them. Input should either be required to be in UTC or should be converted to UTC before they are used. It is however difficult to make tests that show that DateTimes created by a piece of code are indeed UTC.

This is the reason why `RapidCore.Globalization.UtcHelper` exists!

The `UtcHelper` does not replace `DateTime`, it simply provides methods for working with them, that you can actually mock + it provides a very clear **UTC context** - there is no way to get a non-UTC DateTime out of it!

This is a replacement for `DateTime.UtcNow`. The `UtcHelper` becomes a point in your code where you can control the output by mocking it.

#### Examples

- [Test calling thirdParty.ThisIsMyTime with the correct parameters.](../Examples#test-calling-thirdpartythisismytime-with-the-correct-parameters)
- [Get UTC from non UTC datetime using UtcHelper.ToUtc](../Examples#get-utc-from-non-utc-datetime-using-utchelpertoutc)