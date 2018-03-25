using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidCore.Globalization
{
    /// <summary>
    /// Currency definitions as defined by ISO 4217
    /// https://en.wikipedia.org/wiki/ISO_4217
    /// </summary>
    public class Iso4217Currencies
    {
        private readonly List<CurrencyIso4217> currencies = new List<CurrencyIso4217>
        {
            new CurrencyIso4217 { Name = "Afghani", AlphabeticCode = "AFN", NumericCode = 971, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "UAE Dirham", AlphabeticCode = "AED", NumericCode = 784, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Lek", AlphabeticCode = "ALL", NumericCode = 008, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Armenian Dram", AlphabeticCode = "AMD", NumericCode = 051, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Netherlands Antillean Guilder", AlphabeticCode = "ANG", NumericCode = 532, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Kwanza", AlphabeticCode = "AOA", NumericCode = 973, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Argentine Peso", AlphabeticCode = "ARS", NumericCode = 032, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Australian Dollar", AlphabeticCode = "AUD", NumericCode = 036, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Aruban Florin", AlphabeticCode = "AWG", NumericCode = 533, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Azerbaijan Manat", AlphabeticCode = "AZN", NumericCode = 944, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Convertible Mark", AlphabeticCode = "BAM", NumericCode = 977, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Barbados Dollar", AlphabeticCode = "BBD", NumericCode = 052, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Taka", AlphabeticCode = "BDT", NumericCode = 050, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Bulgarian Lev", AlphabeticCode = "BGN", NumericCode = 975, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Bahraini Dinar", AlphabeticCode = "BHD", NumericCode = 048, MinorUnit = 3 },
            new CurrencyIso4217 { Name = "Burundi Franc", AlphabeticCode = "BIF", NumericCode = 108, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Bermudian Dollar", AlphabeticCode = "BMD", NumericCode = 060, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Brunei Dollar", AlphabeticCode = "BND", NumericCode = 096, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Boliviano", AlphabeticCode = "BOB", NumericCode = 068, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Mvdol", AlphabeticCode = "BOV", NumericCode = 984, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Brazilian Real", AlphabeticCode = "BRL", NumericCode = 986, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Bahamian Dollar", AlphabeticCode = "BSD", NumericCode = 044, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Ngultrum", AlphabeticCode = "BTN", NumericCode = 064, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Pula", AlphabeticCode = "BWP", NumericCode = 072, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Belarusian Ruble", AlphabeticCode = "BYN", NumericCode = 933, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Belize Dollar", AlphabeticCode = "BZD", NumericCode = 084, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Canadian Dollar", AlphabeticCode = "CAD", NumericCode = 124, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Congolese Franc", AlphabeticCode = "CDF", NumericCode = 976, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "WIR Euro", AlphabeticCode = "CHE", NumericCode = 947, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Swiss Franc", AlphabeticCode = "CHF", NumericCode = 756, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "WIR Franc", AlphabeticCode = "CHW", NumericCode = 948, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Unidad de Fomento", AlphabeticCode = "CLF", NumericCode = 990, MinorUnit = 4 },
            new CurrencyIso4217 { Name = "Chilean Peso", AlphabeticCode = "CLP", NumericCode = 152, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Yuan Renminbi", AlphabeticCode = "CNY", NumericCode = 156, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Colombian Peso", AlphabeticCode = "COP", NumericCode = 170, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Unidad de Valor Real", AlphabeticCode = "COU", NumericCode = 970, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Costa Rican Colon", AlphabeticCode = "CRC", NumericCode = 188, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Peso Convertible", AlphabeticCode = "CUC", NumericCode = 931, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Cuban Peso", AlphabeticCode = "CUP", NumericCode = 192, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Cabo Verde Escudo", AlphabeticCode = "CVE", NumericCode = 132, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Czech Koruna", AlphabeticCode = "CZK", NumericCode = 203, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Djibouti Franc", AlphabeticCode = "DJF", NumericCode = 262, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Danish Krone", AlphabeticCode = "DKK", NumericCode = 208, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Dominican Peso", AlphabeticCode = "DOP", NumericCode = 214, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Algerian Dinar", AlphabeticCode = "DZD", NumericCode = 012, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Egyptian Pound", AlphabeticCode = "EGP", NumericCode = 818, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Nakfa", AlphabeticCode = "ERN", NumericCode = 232, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Ethiopian Birr", AlphabeticCode = "ETB", NumericCode = 230, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Euro", AlphabeticCode = "EUR", NumericCode = 978, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Fiji Dollar", AlphabeticCode = "FJD", NumericCode = 242, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Falkland Islands Pound", AlphabeticCode = "FKP", NumericCode = 238, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Pound Sterling", AlphabeticCode = "GBP", NumericCode = 826, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Lari", AlphabeticCode = "GEL", NumericCode = 981, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Ghana Cedi", AlphabeticCode = "GHS", NumericCode = 936, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Gibraltar Pound", AlphabeticCode = "GIP", NumericCode = 292, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Dalasi", AlphabeticCode = "GMD", NumericCode = 270, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Guinean Franc", AlphabeticCode = "GNF", NumericCode = 324, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Quetzal", AlphabeticCode = "GTQ", NumericCode = 320, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Guyana Dollar", AlphabeticCode = "GYD", NumericCode = 328, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Hong Kong Dollar", AlphabeticCode = "HKD", NumericCode = 344, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Lempira", AlphabeticCode = "HNL", NumericCode = 340, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Kuna", AlphabeticCode = "HRK", NumericCode = 191, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Gourde", AlphabeticCode = "HTG", NumericCode = 332, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Forint", AlphabeticCode = "HUF", NumericCode = 348, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Rupiah", AlphabeticCode = "IDR", NumericCode = 360, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "New Israeli Sheqel", AlphabeticCode = "ILS", NumericCode = 376, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Indian Rupee", AlphabeticCode = "INR", NumericCode = 356, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Iraqi Dinar", AlphabeticCode = "IQD", NumericCode = 368, MinorUnit = 3 },
            new CurrencyIso4217 { Name = "Iranian Rial", AlphabeticCode = "IRR", NumericCode = 364, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Iceland Krona", AlphabeticCode = "ISK", NumericCode = 352, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Jamaican Dollar", AlphabeticCode = "JMD", NumericCode = 388, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Jordanian Dinar", AlphabeticCode = "JOD", NumericCode = 400, MinorUnit = 3 },
            new CurrencyIso4217 { Name = "Yen", AlphabeticCode = "JPY", NumericCode = 392, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Kenyan Shilling", AlphabeticCode = "KES", NumericCode = 404, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Som", AlphabeticCode = "KGS", NumericCode = 417, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Riel", AlphabeticCode = "KHR", NumericCode = 116, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Comorian Franc ", AlphabeticCode = "KMF", NumericCode = 174, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "North Korean Won", AlphabeticCode = "KPW", NumericCode = 408, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Won", AlphabeticCode = "KRW", NumericCode = 410, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Kuwaiti Dinar", AlphabeticCode = "KWD", NumericCode = 414, MinorUnit = 3 },
            new CurrencyIso4217 { Name = "Cayman Islands Dollar", AlphabeticCode = "KYD", NumericCode = 136, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Tenge", AlphabeticCode = "KZT", NumericCode = 398, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Lao Kip", AlphabeticCode = "LAK", NumericCode = 418, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Lebanese Pound", AlphabeticCode = "LBP", NumericCode = 422, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Sri Lanka Rupee", AlphabeticCode = "LKR", NumericCode = 144, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Liberian Dollar", AlphabeticCode = "LRD", NumericCode = 430, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Loti", AlphabeticCode = "LSL", NumericCode = 426, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Libyan Dinar", AlphabeticCode = "LYD", NumericCode = 434, MinorUnit = 3 },
            new CurrencyIso4217 { Name = "Moroccan Dirham", AlphabeticCode = "MAD", NumericCode = 504, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Moldovan Leu", AlphabeticCode = "MDL", NumericCode = 498, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Malagasy Ariary", AlphabeticCode = "MGA", NumericCode = 969, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Denar", AlphabeticCode = "MKD", NumericCode = 807, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Kyat", AlphabeticCode = "MMK", NumericCode = 104, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Tugrik", AlphabeticCode = "MNT", NumericCode = 496, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Pataca", AlphabeticCode = "MOP", NumericCode = 446, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Ouguiya", AlphabeticCode = "MRU", NumericCode = 929, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Mauritius Rupee", AlphabeticCode = "MUR", NumericCode = 480, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Rufiyaa", AlphabeticCode = "MVR", NumericCode = 462, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Malawi Kwacha", AlphabeticCode = "MWK", NumericCode = 454, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Mexican Peso", AlphabeticCode = "MXN", NumericCode = 484, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Mexican Unidad de Inversion (UDI)", AlphabeticCode = "MXV", NumericCode = 979, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Malaysian Ringgit", AlphabeticCode = "MYR", NumericCode = 458, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Mozambique Metical", AlphabeticCode = "MZN", NumericCode = 943, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Namibia Dollar", AlphabeticCode = "NAD", NumericCode = 516, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Naira", AlphabeticCode = "NGN", NumericCode = 566, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Cordoba Oro", AlphabeticCode = "NIO", NumericCode = 558, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Norwegian Krone", AlphabeticCode = "NOK", NumericCode = 578, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Nepalese Rupee", AlphabeticCode = "NPR", NumericCode = 524, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "New Zealand Dollar", AlphabeticCode = "NZD", NumericCode = 554, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Rial Omani", AlphabeticCode = "OMR", NumericCode = 512, MinorUnit = 3 },
            new CurrencyIso4217 { Name = "Balboa", AlphabeticCode = "PAB", NumericCode = 590, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Sol", AlphabeticCode = "PEN", NumericCode = 604, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Kina", AlphabeticCode = "PGK", NumericCode = 598, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Philippine Piso", AlphabeticCode = "PHP", NumericCode = 608, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Pakistan Rupee", AlphabeticCode = "PKR", NumericCode = 586, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Zloty", AlphabeticCode = "PLN", NumericCode = 985, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Guarani", AlphabeticCode = "PYG", NumericCode = 600, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Qatari Rial", AlphabeticCode = "QAR", NumericCode = 634, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Romanian Leu", AlphabeticCode = "RON", NumericCode = 946, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Serbian Dinar", AlphabeticCode = "RSD", NumericCode = 941, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Russian Ruble", AlphabeticCode = "RUB", NumericCode = 643, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Rwanda Franc", AlphabeticCode = "RWF", NumericCode = 646, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Saudi Riyal", AlphabeticCode = "SAR", NumericCode = 682, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Solomon Islands Dollar", AlphabeticCode = "SBD", NumericCode = 090, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Seychelles Rupee", AlphabeticCode = "SCR", NumericCode = 690, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Sudanese Pound", AlphabeticCode = "SDG", NumericCode = 938, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Swedish Krona", AlphabeticCode = "SEK", NumericCode = 752, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Singapore Dollar", AlphabeticCode = "SGD", NumericCode = 702, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Saint Helena Pound", AlphabeticCode = "SHP", NumericCode = 654, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Leone", AlphabeticCode = "SLL", NumericCode = 694, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Somali Shilling", AlphabeticCode = "SOS", NumericCode = 706, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Surinam Dollar", AlphabeticCode = "SRD", NumericCode = 968, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "South Sudanese Pound", AlphabeticCode = "SSP", NumericCode = 728, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Dobra", AlphabeticCode = "STN", NumericCode = 930, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "El Salvador Colon", AlphabeticCode = "SVC", NumericCode = 222, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Syrian Pound", AlphabeticCode = "SYP", NumericCode = 760, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Lilangeni", AlphabeticCode = "SZL", NumericCode = 748, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Baht", AlphabeticCode = "THB", NumericCode = 764, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Somoni", AlphabeticCode = "TJS", NumericCode = 972, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Turkmenistan New Manat", AlphabeticCode = "TMT", NumericCode = 934, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Tunisian Dinar", AlphabeticCode = "TND", NumericCode = 788, MinorUnit = 3 },
            new CurrencyIso4217 { Name = "Pa’anga", AlphabeticCode = "TOP", NumericCode = 776, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Turkish Lira", AlphabeticCode = "TRY", NumericCode = 949, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Trinidad and Tobago Dollar", AlphabeticCode = "TTD", NumericCode = 780, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "New Taiwan Dollar", AlphabeticCode = "TWD", NumericCode = 901, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Tanzanian Shilling", AlphabeticCode = "TZS", NumericCode = 834, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Hryvnia", AlphabeticCode = "UAH", NumericCode = 980, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Uganda Shilling", AlphabeticCode = "UGX", NumericCode = 800, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "US Dollar", AlphabeticCode = "USD", NumericCode = 840, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "US Dollar (Next day)", AlphabeticCode = "USN", NumericCode = 997, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Uruguay Peso en Unidades Indexadas (URUIURUI)", AlphabeticCode = "UYI", NumericCode = 940, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Peso Uruguayo", AlphabeticCode = "UYU", NumericCode = 858, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Uzbekistan Sum", AlphabeticCode = "UZS", NumericCode = 860, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Bolívar", AlphabeticCode = "VEF", NumericCode = 937, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Dong", AlphabeticCode = "VND", NumericCode = 704, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Vatu", AlphabeticCode = "VUV", NumericCode = 548, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Tala", AlphabeticCode = "WST", NumericCode = 882, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "CFA Franc BEAC", AlphabeticCode = "XAF", NumericCode = 950, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Silver", AlphabeticCode = "XAG", NumericCode = 961, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Gold", AlphabeticCode = "XAU", NumericCode = 959, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "East Caribbean Dollar", AlphabeticCode = "XCD", NumericCode = 951, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "SDR (Special Drawing Right)", AlphabeticCode = "XDR", NumericCode = 960, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "CFA Franc BCEAO", AlphabeticCode = "XOF", NumericCode = 952, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Palladium", AlphabeticCode = "XPD", NumericCode = 964, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "CFP Franc", AlphabeticCode = "XPF", NumericCode = 953, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Platinum", AlphabeticCode = "XPT", NumericCode = 962, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Sucre", AlphabeticCode = "XSU", NumericCode = 994, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "Codes specifically reserved for testing purposes", AlphabeticCode = "XTS", NumericCode = 963, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "ADB Unit of Account", AlphabeticCode = "XUA", NumericCode = 965, MinorUnit = 0 },
            new CurrencyIso4217 { Name = "The codes assigned for transactions where no currency is involved", AlphabeticCode = "XXX", NumericCode = 999, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Yemeni Rial", AlphabeticCode = "YER", NumericCode = 886, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Rand", AlphabeticCode = "ZAR", NumericCode = 710, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Zambian Kwacha", AlphabeticCode = "ZMW", NumericCode = 967, MinorUnit = 2 },
            new CurrencyIso4217 { Name = "Zimbabwe Dollar", AlphabeticCode = "ZWL", NumericCode = 932, MinorUnit = 2 }
        };

        public virtual IReadOnlyList<CurrencyIso4217> GetAll()
        {
            return currencies;
        }

        /// <summary>
        /// Get currency by name
        /// </summary>
        /// <param name="currencyName">currency name</param>
        /// <returns>The matching currency or null</returns>
        public virtual CurrencyIso4217 GetCurrencyByCurrencyName(string currencyName)
        {
            var uppered = currencyName.ToUpper();

            return currencies.SingleOrDefault(x => x.Name.ToUpper() == uppered);
        }

        /// <summary>
        /// Get currency by name
        /// </summary>
        /// <param name="alphabeticCode">currency name</param>
        /// <returns>The matching currency or null</returns>
        public virtual CurrencyIso4217 GetCurrencyByAlphabeticCode(string alphabeticCode)
        {
            var uppered = alphabeticCode.ToUpper();
            return currencies.SingleOrDefault(x => x.AlphabeticCode.ToUpper() == uppered);
        }

        /// <summary>
        /// Get currency by numeric code
        /// </summary>
        /// <param name="numericCode">currency name</param>
        /// <returns>The matching currency or null</returns>
        public virtual CurrencyIso4217 GetCurrencyByNumericCode(int numericCode)
        {
            return currencies.SingleOrDefault(x => x.NumericCode == numericCode);
        }

    }
}
