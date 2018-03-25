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
            new CurrencyIso4217 { NameEnglish = "Afghani", CodeAlpha = "AFN", CodeNumeric = 971, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "UAE Dirham", CodeAlpha = "AED", CodeNumeric = 784, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Lek", CodeAlpha = "ALL", CodeNumeric = 008, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Armenian Dram", CodeAlpha = "AMD", CodeNumeric = 051, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Netherlands Antillean Guilder", CodeAlpha = "ANG", CodeNumeric = 532, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Kwanza", CodeAlpha = "AOA", CodeNumeric = 973, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Argentine Peso", CodeAlpha = "ARS", CodeNumeric = 032, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Australian Dollar", CodeAlpha = "AUD", CodeNumeric = 036, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Aruban Florin", CodeAlpha = "AWG", CodeNumeric = 533, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Azerbaijan Manat", CodeAlpha = "AZN", CodeNumeric = 944, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Convertible Mark", CodeAlpha = "BAM", CodeNumeric = 977, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Barbados Dollar", CodeAlpha = "BBD", CodeNumeric = 052, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Taka", CodeAlpha = "BDT", CodeNumeric = 050, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Bulgarian Lev", CodeAlpha = "BGN", CodeNumeric = 975, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Bahraini Dinar", CodeAlpha = "BHD", CodeNumeric = 048, MinorUnit = 3 },
            new CurrencyIso4217 { NameEnglish = "Burundi Franc", CodeAlpha = "BIF", CodeNumeric = 108, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Bermudian Dollar", CodeAlpha = "BMD", CodeNumeric = 060, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Brunei Dollar", CodeAlpha = "BND", CodeNumeric = 096, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Boliviano", CodeAlpha = "BOB", CodeNumeric = 068, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Mvdol", CodeAlpha = "BOV", CodeNumeric = 984, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Brazilian Real", CodeAlpha = "BRL", CodeNumeric = 986, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Bahamian Dollar", CodeAlpha = "BSD", CodeNumeric = 044, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Ngultrum", CodeAlpha = "BTN", CodeNumeric = 064, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Pula", CodeAlpha = "BWP", CodeNumeric = 072, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Belarusian Ruble", CodeAlpha = "BYN", CodeNumeric = 933, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Belize Dollar", CodeAlpha = "BZD", CodeNumeric = 084, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Canadian Dollar", CodeAlpha = "CAD", CodeNumeric = 124, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Congolese Franc", CodeAlpha = "CDF", CodeNumeric = 976, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "WIR Euro", CodeAlpha = "CHE", CodeNumeric = 947, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Swiss Franc", CodeAlpha = "CHF", CodeNumeric = 756, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "WIR Franc", CodeAlpha = "CHW", CodeNumeric = 948, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Unidad de Fomento", CodeAlpha = "CLF", CodeNumeric = 990, MinorUnit = 4 },
            new CurrencyIso4217 { NameEnglish = "Chilean Peso", CodeAlpha = "CLP", CodeNumeric = 152, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Yuan Renminbi", CodeAlpha = "CNY", CodeNumeric = 156, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Colombian Peso", CodeAlpha = "COP", CodeNumeric = 170, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Unidad de Valor Real", CodeAlpha = "COU", CodeNumeric = 970, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Costa Rican Colon", CodeAlpha = "CRC", CodeNumeric = 188, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Peso Convertible", CodeAlpha = "CUC", CodeNumeric = 931, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Cuban Peso", CodeAlpha = "CUP", CodeNumeric = 192, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Cabo Verde Escudo", CodeAlpha = "CVE", CodeNumeric = 132, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Czech Koruna", CodeAlpha = "CZK", CodeNumeric = 203, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Djibouti Franc", CodeAlpha = "DJF", CodeNumeric = 262, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Danish Krone", CodeAlpha = "DKK", CodeNumeric = 208, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Dominican Peso", CodeAlpha = "DOP", CodeNumeric = 214, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Algerian Dinar", CodeAlpha = "DZD", CodeNumeric = 012, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Egyptian Pound", CodeAlpha = "EGP", CodeNumeric = 818, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Nakfa", CodeAlpha = "ERN", CodeNumeric = 232, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Ethiopian Birr", CodeAlpha = "ETB", CodeNumeric = 230, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Euro", CodeAlpha = "EUR", CodeNumeric = 978, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Fiji Dollar", CodeAlpha = "FJD", CodeNumeric = 242, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Falkland Islands Pound", CodeAlpha = "FKP", CodeNumeric = 238, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Pound Sterling", CodeAlpha = "GBP", CodeNumeric = 826, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Lari", CodeAlpha = "GEL", CodeNumeric = 981, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Ghana Cedi", CodeAlpha = "GHS", CodeNumeric = 936, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Gibraltar Pound", CodeAlpha = "GIP", CodeNumeric = 292, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Dalasi", CodeAlpha = "GMD", CodeNumeric = 270, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Guinean Franc", CodeAlpha = "GNF", CodeNumeric = 324, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Quetzal", CodeAlpha = "GTQ", CodeNumeric = 320, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Guyana Dollar", CodeAlpha = "GYD", CodeNumeric = 328, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Hong Kong Dollar", CodeAlpha = "HKD", CodeNumeric = 344, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Lempira", CodeAlpha = "HNL", CodeNumeric = 340, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Kuna", CodeAlpha = "HRK", CodeNumeric = 191, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Gourde", CodeAlpha = "HTG", CodeNumeric = 332, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Forint", CodeAlpha = "HUF", CodeNumeric = 348, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Rupiah", CodeAlpha = "IDR", CodeNumeric = 360, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "New Israeli Sheqel", CodeAlpha = "ILS", CodeNumeric = 376, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Indian Rupee", CodeAlpha = "INR", CodeNumeric = 356, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Iraqi Dinar", CodeAlpha = "IQD", CodeNumeric = 368, MinorUnit = 3 },
            new CurrencyIso4217 { NameEnglish = "Iranian Rial", CodeAlpha = "IRR", CodeNumeric = 364, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Iceland Krona", CodeAlpha = "ISK", CodeNumeric = 352, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Jamaican Dollar", CodeAlpha = "JMD", CodeNumeric = 388, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Jordanian Dinar", CodeAlpha = "JOD", CodeNumeric = 400, MinorUnit = 3 },
            new CurrencyIso4217 { NameEnglish = "Yen", CodeAlpha = "JPY", CodeNumeric = 392, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Kenyan Shilling", CodeAlpha = "KES", CodeNumeric = 404, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Som", CodeAlpha = "KGS", CodeNumeric = 417, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Riel", CodeAlpha = "KHR", CodeNumeric = 116, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Comorian Franc ", CodeAlpha = "KMF", CodeNumeric = 174, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "North Korean Won", CodeAlpha = "KPW", CodeNumeric = 408, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Won", CodeAlpha = "KRW", CodeNumeric = 410, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Kuwaiti Dinar", CodeAlpha = "KWD", CodeNumeric = 414, MinorUnit = 3 },
            new CurrencyIso4217 { NameEnglish = "Cayman Islands Dollar", CodeAlpha = "KYD", CodeNumeric = 136, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Tenge", CodeAlpha = "KZT", CodeNumeric = 398, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Lao Kip", CodeAlpha = "LAK", CodeNumeric = 418, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Lebanese Pound", CodeAlpha = "LBP", CodeNumeric = 422, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Sri Lanka Rupee", CodeAlpha = "LKR", CodeNumeric = 144, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Liberian Dollar", CodeAlpha = "LRD", CodeNumeric = 430, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Loti", CodeAlpha = "LSL", CodeNumeric = 426, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Libyan Dinar", CodeAlpha = "LYD", CodeNumeric = 434, MinorUnit = 3 },
            new CurrencyIso4217 { NameEnglish = "Moroccan Dirham", CodeAlpha = "MAD", CodeNumeric = 504, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Moldovan Leu", CodeAlpha = "MDL", CodeNumeric = 498, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Malagasy Ariary", CodeAlpha = "MGA", CodeNumeric = 969, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Denar", CodeAlpha = "MKD", CodeNumeric = 807, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Kyat", CodeAlpha = "MMK", CodeNumeric = 104, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Tugrik", CodeAlpha = "MNT", CodeNumeric = 496, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Pataca", CodeAlpha = "MOP", CodeNumeric = 446, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Ouguiya", CodeAlpha = "MRU", CodeNumeric = 929, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Mauritius Rupee", CodeAlpha = "MUR", CodeNumeric = 480, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Rufiyaa", CodeAlpha = "MVR", CodeNumeric = 462, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Malawi Kwacha", CodeAlpha = "MWK", CodeNumeric = 454, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Mexican Peso", CodeAlpha = "MXN", CodeNumeric = 484, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Mexican Unidad de Inversion (UDI)", CodeAlpha = "MXV", CodeNumeric = 979, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Malaysian Ringgit", CodeAlpha = "MYR", CodeNumeric = 458, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Mozambique Metical", CodeAlpha = "MZN", CodeNumeric = 943, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Namibia Dollar", CodeAlpha = "NAD", CodeNumeric = 516, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Naira", CodeAlpha = "NGN", CodeNumeric = 566, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Cordoba Oro", CodeAlpha = "NIO", CodeNumeric = 558, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Norwegian Krone", CodeAlpha = "NOK", CodeNumeric = 578, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Nepalese Rupee", CodeAlpha = "NPR", CodeNumeric = 524, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "New Zealand Dollar", CodeAlpha = "NZD", CodeNumeric = 554, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Rial Omani", CodeAlpha = "OMR", CodeNumeric = 512, MinorUnit = 3 },
            new CurrencyIso4217 { NameEnglish = "Balboa", CodeAlpha = "PAB", CodeNumeric = 590, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Sol", CodeAlpha = "PEN", CodeNumeric = 604, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Kina", CodeAlpha = "PGK", CodeNumeric = 598, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Philippine Piso", CodeAlpha = "PHP", CodeNumeric = 608, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Pakistan Rupee", CodeAlpha = "PKR", CodeNumeric = 586, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Zloty", CodeAlpha = "PLN", CodeNumeric = 985, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Guarani", CodeAlpha = "PYG", CodeNumeric = 600, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Qatari Rial", CodeAlpha = "QAR", CodeNumeric = 634, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Romanian Leu", CodeAlpha = "RON", CodeNumeric = 946, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Serbian Dinar", CodeAlpha = "RSD", CodeNumeric = 941, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Russian Ruble", CodeAlpha = "RUB", CodeNumeric = 643, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Rwanda Franc", CodeAlpha = "RWF", CodeNumeric = 646, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Saudi Riyal", CodeAlpha = "SAR", CodeNumeric = 682, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Solomon Islands Dollar", CodeAlpha = "SBD", CodeNumeric = 090, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Seychelles Rupee", CodeAlpha = "SCR", CodeNumeric = 690, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Sudanese Pound", CodeAlpha = "SDG", CodeNumeric = 938, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Swedish Krona", CodeAlpha = "SEK", CodeNumeric = 752, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Singapore Dollar", CodeAlpha = "SGD", CodeNumeric = 702, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Saint Helena Pound", CodeAlpha = "SHP", CodeNumeric = 654, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Leone", CodeAlpha = "SLL", CodeNumeric = 694, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Somali Shilling", CodeAlpha = "SOS", CodeNumeric = 706, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Surinam Dollar", CodeAlpha = "SRD", CodeNumeric = 968, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "South Sudanese Pound", CodeAlpha = "SSP", CodeNumeric = 728, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Dobra", CodeAlpha = "STN", CodeNumeric = 930, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "El Salvador Colon", CodeAlpha = "SVC", CodeNumeric = 222, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Syrian Pound", CodeAlpha = "SYP", CodeNumeric = 760, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Lilangeni", CodeAlpha = "SZL", CodeNumeric = 748, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Baht", CodeAlpha = "THB", CodeNumeric = 764, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Somoni", CodeAlpha = "TJS", CodeNumeric = 972, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Turkmenistan New Manat", CodeAlpha = "TMT", CodeNumeric = 934, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Tunisian Dinar", CodeAlpha = "TND", CodeNumeric = 788, MinorUnit = 3 },
            new CurrencyIso4217 { NameEnglish = "Pa’anga", CodeAlpha = "TOP", CodeNumeric = 776, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Turkish Lira", CodeAlpha = "TRY", CodeNumeric = 949, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Trinidad and Tobago Dollar", CodeAlpha = "TTD", CodeNumeric = 780, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "New Taiwan Dollar", CodeAlpha = "TWD", CodeNumeric = 901, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Tanzanian Shilling", CodeAlpha = "TZS", CodeNumeric = 834, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Hryvnia", CodeAlpha = "UAH", CodeNumeric = 980, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Uganda Shilling", CodeAlpha = "UGX", CodeNumeric = 800, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "US Dollar", CodeAlpha = "USD", CodeNumeric = 840, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "US Dollar (Next day)", CodeAlpha = "USN", CodeNumeric = 997, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Uruguay Peso en Unidades Indexadas (URUIURUI)", CodeAlpha = "UYI", CodeNumeric = 940, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Peso Uruguayo", CodeAlpha = "UYU", CodeNumeric = 858, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Uzbekistan Sum", CodeAlpha = "UZS", CodeNumeric = 860, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Bolívar", CodeAlpha = "VEF", CodeNumeric = 937, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Dong", CodeAlpha = "VND", CodeNumeric = 704, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Vatu", CodeAlpha = "VUV", CodeNumeric = 548, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Tala", CodeAlpha = "WST", CodeNumeric = 882, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "CFA Franc BEAC", CodeAlpha = "XAF", CodeNumeric = 950, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Silver", CodeAlpha = "XAG", CodeNumeric = 961, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Gold", CodeAlpha = "XAU", CodeNumeric = 959, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "East Caribbean Dollar", CodeAlpha = "XCD", CodeNumeric = 951, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "SDR (Special Drawing Right)", CodeAlpha = "XDR", CodeNumeric = 960, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "CFA Franc BCEAO", CodeAlpha = "XOF", CodeNumeric = 952, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Palladium", CodeAlpha = "XPD", CodeNumeric = 964, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "CFP Franc", CodeAlpha = "XPF", CodeNumeric = 953, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Platinum", CodeAlpha = "XPT", CodeNumeric = 962, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Sucre", CodeAlpha = "XSU", CodeNumeric = 994, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "Codes specifically reserved for testing purposes", CodeAlpha = "XTS", CodeNumeric = 963, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "ADB Unit of Account", CodeAlpha = "XUA", CodeNumeric = 965, MinorUnit = 0 },
            new CurrencyIso4217 { NameEnglish = "The codes assigned for transactions where no currency is involved", CodeAlpha = "XXX", CodeNumeric = 999, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Yemeni Rial", CodeAlpha = "YER", CodeNumeric = 886, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Rand", CodeAlpha = "ZAR", CodeNumeric = 710, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Zambian Kwacha", CodeAlpha = "ZMW", CodeNumeric = 967, MinorUnit = 2 },
            new CurrencyIso4217 { NameEnglish = "Zimbabwe Dollar", CodeAlpha = "ZWL", CodeNumeric = 932, MinorUnit = 2 }
        };

        public virtual IReadOnlyList<CurrencyIso4217> GetAll()
        {
            return currencies;
        }

        /// <summary>
        /// Get currency by either alpha or numeric code
        /// </summary>
        /// <param name="alphaOrNumeric">Alphabetic or numeric currency code</param>
        /// <returns>The matching currency or <c>null</c></returns>
        public virtual CurrencyIso4217 Get(string alphaOrNumeric)
        {
            // the input could be numeric, but sent in as a string
            if (int.TryParse(alphaOrNumeric, out int numeric))
            {
                return Get(numeric);
            }
            
            var uppered = alphaOrNumeric.ToUpper();
            return currencies.FirstOrDefault(x => x.CodeAlpha== uppered);
        }

        /// <summary>
        /// Get currency by numeric code
        /// </summary>
        /// <param name="numeric">Numeric currency code</param>
        /// <returns>The matching currency or <c>null</c></returns>
        public virtual CurrencyIso4217 Get(int numeric)
        {
            return currencies.SingleOrDefault(x => x.CodeNumeric == numeric);
        }
        
        /// <summary>
        /// Check whether two given currency codes refer to the same currency
        /// </summary>
        /// <param name="aAlphaOrNumeric">Currency A</param>
        /// <param name="bAlphaOrNumeric">Currency B</param>
        /// <returns>Whether or not the currency codes refer to the same currency. If one or both currencies codes are invalid, <c>false</c> is returned.</returns>
        public virtual bool Matches(string aAlphaOrNumeric, string bAlphaOrNumeric)
        {
            var a = Get(aAlphaOrNumeric);

            if (a == default(CurrencyIso4217))
            {
                return false;
            }

            return a.Is(bAlphaOrNumeric);
        }
    }
}
