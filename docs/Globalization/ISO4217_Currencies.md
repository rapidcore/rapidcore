# ISO 4217 Currencies

Currencies can be identified by two different codes (alpha or numeric) and they can have different minor units.

The `minor unit` is the smallest possible unit of the currency. Most currencies have a minor unit of 2, meaning that the smallest amount available is 1/100th - e.g. for USD this would be 1 cent aka 0.01 USD.

We have implemented [ISO 4217](https://www.iso.org/iso-4217-currency-codes.html).

All of this lives in the `RapidCore.Globalization` namespace.

#### Examples

- [Get currency based on currency code](../Examples#get-currency-based-on-currency-code)
- [Code branch based on currency code](../Examples#code-branch-based-on-currency-code)
- [Validating a given currency code](../Examples#validating-a-given-currency-code)