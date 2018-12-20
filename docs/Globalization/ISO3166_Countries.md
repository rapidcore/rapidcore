# ISO 3166 Countries

Country codes can have multiple forms. In order to address this, we have implemented a list of countries as defined by [ISO 3166](https://www.iso.org/iso-3166-country-codes.html) and tools for seamless "translation" between the codes defined in the ISO; namely:

- alpha-2 (two letter representation)
- alpha-3 (three letter representation)
- numeric code

All of this lives in the `RapidCore.Globalization` namespace.

#### Examples

- [Get country based on country code](../Examples#get-country-based-on-country-code)
- [Code branch based on country code](../Examples#code-branch-based-on-country-code)
- [Validating a given country code](../Examples#validating-a-given-country-code)