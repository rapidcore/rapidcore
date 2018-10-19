# ISO 3166 Countries

Country codes can have multiple forms. In order to address this, we have implemented a list of countries as defined by [ISO 3166](https://www.iso.org/iso-3166-country-codes.html) and tools for seamless "translation" between the codes defined in the ISO; namely:

- alpha-2 (two letter representation)
- alpha-3 (three letter representation)
- numeric code

All of this lives in the `RapidCore.Globalization` namespace.


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