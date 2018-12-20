# Examples

## Test calling thirdParty.ThisIsMyTime with the correct parameters.

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

## Get UTC from non UTC datetime using UtcHelper.ToUtc

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

## Get country based on country code

This example shows how to get a country based on country code. The only requirement is that the provided country code is a valid code from ISO 3166 - it does not matter if it is alpha-2, alpha-3 or numeric.

```csharp
using RapidCore.Globalization;

public class SomeHandler()
{
    private readonly Iso3166Countries countries;

    public SomeHandler(Iso3166Countries countries)
    {
        this.countries = countries;
    }

    public void SomeAction(string providedCountryCode)
    {
        /**
         * The .Get(string) method accepts all 3 country
         * code formats (alpha-2, alpha-3 and numeric).
         *
         * It also has an overload if you receive the country
         * code as an int
         */
        var country = countries.Get(providedCountryCode);

        if (country == default(CountryIso3166))
        {
            throw new ArgumentException($"Invalid country code {providedCountryCode}");
        }

        var twoLetterAlpha = country.CodeAlpha2;
    }
}
```

## Code branch based on country code

Say that you have some processing in your code, where you need to do something different if the country code of something is a specific country. A typical example could be something with logistics.

```csharp
using RapidCore.Globalization;

public class SomeProcessor()
{
    private readonly Iso3166Countries countries;

    public SomeProcessor(Iso3166Countries countries)
    {
        this.countries = countries;
    }

    public void LotsOfHardWork(SomeThing workOnThis)
    {
        // ..lots of work...

        /**
         * If this thing has something to do with Sweden,
         * we need to do something different
         */
        if (countries.Matches("swe", workOnThis.CountryCode)) // again, input any form of ISO 3166 country code..
        {
            // do swedish stuff
        }
        else
        {
            // rest of the world
        }
    }
}
```

## Validating a given country code

If you need to validate whether a given country code is valid or not you have 2 options depending on what else you need.

1. Use `IsValidCountryCode(string)` if you do not need the country instance, but just needs to know whether the code is valid
2. Use `Get(string)` and check for null, if you need the country instance

```csharp
using RapidCore.Globalization;

public class SomeHandler()
{
    private readonly Iso3166Countries countries;

    public SomeHandler(Iso3166Countries countries)
    {
        this.countries = countries;
    }

    public void Myes(SomeThing input)
    {
        /**
         * OPTION 1
         *
         * If you just need to know whether a given country code
         * is valid, but you do not need the actual country instance
         * you can use the "IsValidCountryCode" convenience method.
         */
        if (!countries.IsValidCountryCode(input.CountryCode))
        {
            throw new ArgumentException($"{input.CountryCode} is not a valid ISO 3166 country code");
        }

        /**
         * OPTION 2
         *
         * If on the other hand, you actually need information about
         * the country, you might as well just use "Get" and check for null
         */
        var country = countries.Get(input.CountryCode);

        if (country == default(Iso3166Country))
        {
            throw new ArgumentException($"{input.CountryCode} is not a valid ISO 3166 country code");
        }

        var someOtherThing = new OtherThing
        {
            CountryCode = country.CodeNumeric
        };
    }
}
```

## Get currency based on currency code

This example shows how to get a currency based on currency code. The only requirement is that the provided currency code is a valid code from ISO 4217 - it does not matter if it is alpha or numeric.

```csharp
using RapidCore.Globalization;

public class SomeHandler()
{
    private readonly Iso4217Currencies currencies;

    public SomeHandler(Iso4217Currencies currencies)
    {
        this.currencies = currencies;
    }

    public void SomeAction(string providedCurrencyCode)
    {
        /**
         * The .Get(string) method accepts both currency
         * code formats (alpha and numeric).
         *
         * It also has an overload if you receive the currency
         * code as an int
         */
        var currency = currencies.Get(providedCurrencyCode);

        if (currency == default(CurrencyIso4217))
        {
            throw new ArgumentException($"Invalid currency code {providedCurrencyCode}");
        }

        var name = currency.NameEnglish;
    }
}
```

## Code branch based on currency code

Say that you have some processing in your code, where you need to do something different if the currency code of something is a specific currency. An example could be some sort of "bank account" lookup.

```csharp
using RapidCore.Globalization;

public class SomeProcessor()
{
    private readonly Iso4217Currencies currencies;

    public SomeProcessor(Iso4217Currencies currencies)
    {
        this.currencies = currencies;
    }

    public void LotsOfHardWork(SomeThing workOnThis)
    {
        // ..lots of work...

        /**
         * If this thing has something to do with Euro,
         * we need to do something different
         */
        if (currencies.Matches("eur", workOnThis.CurrencyCode)) // again, input any form of ISO 4217 currency code..
        {
            // do Euro stuff
        }
        else
        {
            // any other currency
        }
    }
}
```

## Validating a given currency code

If you need to validate whether a given currency code is valid or not you have 2 options depending on what else you need.

1. Use `IsValidCurrencyCode(string)` if you do not need the currency instance, but just needs to know whether the code is valid
2. Use `Get(string)` and check for null, if you need the currency instance

```csharp
using RapidCore.Globalization;

public class SomeHandler()
{
    private readonly Iso4217Currencies currencies;

    public SomeHandler(Iso4217Currencies currencies)
    {
        this.currencies = currencies;
    }

    public void Myes(SomeThing input)
    {
        /**
         * OPTION 1
         *
         * If you just need to know whether a given currency code
         * is valid, but you do not need the actual currency instance
         * you can use the "IsValidCurrencyCode" convenience method.
         */
        if (!currencies.IsValidCurrencyCode(input.CurrencyCode))
        {
            throw new ArgumentException($"{input.CurrencyCode} is not a valid ISO 4217 currency code");
        }

        /**
         * OPTION 2
         *
         * If on the other hand, you actually need information about
         * the currency, you might as well just use "Get" and check for null
         */
        var currency = currencies.Get(input.CurrencyCode);

        if (currency == default(Iso4217Currency))
        {
            throw new ArgumentException($"{input.CurrencyCode} is not a valid ISO 4217 currency code");
        }

        var someOtherThing = new OtherThing
        {
            CurrencyCode = currency.CodeNumeric
        };
    }
}
```