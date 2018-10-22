# ISO 4217 Currencies

Currencies can be identified by two different codes (alpha or numeric) and they can have different minor units.

The `minor unit` is the smallest possible unit of the currency. Most currencies have a minor unit of 2, meaning that the smallest amount available is 1/100th - e.g. for USD this would be 1 cent aka 0.01 USD.

We have implemented [ISO 4217](https://www.iso.org/iso-4217-currency-codes.html).

All of this lives in the `RapidCore.Globalization` namespace.


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