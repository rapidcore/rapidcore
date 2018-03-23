using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            new CurrencyIso4217 { CurrencyName = "Afghani", AlphabeticCode = "AFN",  NumericCode = 971},
            new CurrencyIso4217 { CurrencyName = "Euro", AlphabeticCode = "EUR" , NumericCode = 978},
            new CurrencyIso4217 { CurrencyName = "Lek", AlphabeticCode = "ALL"  , NumericCode = 008},
            new CurrencyIso4217 { CurrencyName = "Algerian Dinar", AlphabeticCode = "DZD"   , NumericCode = 012},
            new CurrencyIso4217 { CurrencyName = "US Dollar", AlphabeticCode = "USD"    , NumericCode = 840},
            new CurrencyIso4217 { CurrencyName = "Kwanza", AlphabeticCode = "AOA"   , NumericCode = 973},
            new CurrencyIso4217 { CurrencyName = "East Caribbean Dollar", AlphabeticCode = "XCD"    , NumericCode = 951},
            new CurrencyIso4217 { CurrencyName = "Argentine Peso", AlphabeticCode = "ARS"   , NumericCode = 032},
            new CurrencyIso4217 { CurrencyName = "Armenian Dram", AlphabeticCode = "AMD"    , NumericCode = 051},
            new CurrencyIso4217 { CurrencyName = "Aruban Florin", AlphabeticCode = "AWG"    , NumericCode = 533},
            new CurrencyIso4217 { CurrencyName = "Australian Dollar", AlphabeticCode = "AUD"    , NumericCode = 036},
            new CurrencyIso4217 { CurrencyName = "Azerbaijan Manat", AlphabeticCode = "AZN" , NumericCode = 944},
            new CurrencyIso4217 { CurrencyName = "Bahamian Dollar", AlphabeticCode = "BSD"  , NumericCode = 044},
            new CurrencyIso4217 { CurrencyName = "Bahraini Dinar", AlphabeticCode = "BHD"   , NumericCode = 048},
            new CurrencyIso4217 { CurrencyName = "Taka", AlphabeticCode = "BDT" , NumericCode = 050},
            new CurrencyIso4217 { CurrencyName = "Barbados Dollar", AlphabeticCode = "BBD"  , NumericCode = 052},
            new CurrencyIso4217 { CurrencyName = "Belarusian Ruble", AlphabeticCode = "BYN" , NumericCode = 933},
            new CurrencyIso4217 { CurrencyName = "Belize Dollar", AlphabeticCode = "BZD"    , NumericCode = 084},
            new CurrencyIso4217 { CurrencyName = "CFA Franc BCEAO", AlphabeticCode = "XOF"  , NumericCode = 952},
            new CurrencyIso4217 { CurrencyName = "Bermudian Dollar", AlphabeticCode = "BMD" , NumericCode = 060},
            new CurrencyIso4217 { CurrencyName = "Indian Rupee", AlphabeticCode = "INR" , NumericCode = 356},
            new CurrencyIso4217 { CurrencyName = "Ngultrum", AlphabeticCode = "BTN" , NumericCode = 064},
            new CurrencyIso4217 { CurrencyName = "Boliviano", AlphabeticCode = "BOB"    , NumericCode = 068},
            new CurrencyIso4217 { CurrencyName = "Mvdol", AlphabeticCode = "BOV"    , NumericCode = 984},
            new CurrencyIso4217 { CurrencyName = "Convertible Mark", AlphabeticCode = "BAM" , NumericCode = 977},
            new CurrencyIso4217 { CurrencyName = "Pula", AlphabeticCode = "BWP" , NumericCode = 072},
            new CurrencyIso4217 { CurrencyName = "Norwegian Krone", AlphabeticCode = "NOK"  , NumericCode = 578},
            new CurrencyIso4217 { CurrencyName = "Brazilian Real", AlphabeticCode = "BRL"   , NumericCode = 986},
            new CurrencyIso4217 { CurrencyName = "Brunei Dollar", AlphabeticCode = "BND"    , NumericCode = 096},
            new CurrencyIso4217 { CurrencyName = "Bulgarian Lev", AlphabeticCode = "BGN"    , NumericCode = 975},
            new CurrencyIso4217 { CurrencyName = "Burundi Franc", AlphabeticCode = "BIF"    , NumericCode = 108},
            new CurrencyIso4217 { CurrencyName = "Cabo Verde Escudo", AlphabeticCode = "CVE"    , NumericCode = 132},
            new CurrencyIso4217 { CurrencyName = "Riel", AlphabeticCode = "KHR" , NumericCode = 116},
            new CurrencyIso4217 { CurrencyName = "CFA Franc BEAC", AlphabeticCode = "XAF"   , NumericCode = 950},
            new CurrencyIso4217 { CurrencyName = "Canadian Dollar", AlphabeticCode = "CAD"  , NumericCode = 124},
            new CurrencyIso4217 { CurrencyName = "Cayman Islands Dollar", AlphabeticCode = "KYD"    , NumericCode = 136},
            new CurrencyIso4217 { CurrencyName = "Chilean Peso", AlphabeticCode = "CLP" , NumericCode = 152},
            new CurrencyIso4217 { CurrencyName = "Unidad de Fomento", AlphabeticCode = "CLF"    , NumericCode = 990},
            new CurrencyIso4217 { CurrencyName = "Yuan Renminbi", AlphabeticCode = "CNY"    , NumericCode = 156},
            new CurrencyIso4217 { CurrencyName = "Colombian Peso", AlphabeticCode = "COP"   , NumericCode = 170},
            new CurrencyIso4217 { CurrencyName = "Unidad de Valor Real", AlphabeticCode = "COU" , NumericCode = 970},
            new CurrencyIso4217 { CurrencyName = "Comorian Franc ", AlphabeticCode = "KMF"  , NumericCode = 174},
            new CurrencyIso4217 { CurrencyName = "Congolese Franc", AlphabeticCode = "CDF"  , NumericCode = 976},
            new CurrencyIso4217 { CurrencyName = "New Zealand Dollar", AlphabeticCode = "NZD"   , NumericCode = 554},
            new CurrencyIso4217 { CurrencyName = "Costa Rican Colon", AlphabeticCode = "CRC"    , NumericCode = 188},
            new CurrencyIso4217 { CurrencyName = "Kuna", AlphabeticCode = "HRK" , NumericCode = 191},
            new CurrencyIso4217 { CurrencyName = "Cuban Peso", AlphabeticCode = "CUP"   , NumericCode = 192},
            new CurrencyIso4217 { CurrencyName = "Peso Convertible", AlphabeticCode = "CUC" , NumericCode = 931},
            new CurrencyIso4217 { CurrencyName = "Netherlands Antillean Guilder", AlphabeticCode = "ANG"    , NumericCode = 532},
            new CurrencyIso4217 { CurrencyName = "Czech Koruna", AlphabeticCode = "CZK" , NumericCode = 203},
            new CurrencyIso4217 { CurrencyName = "Danish Krone", AlphabeticCode = "DKK" , NumericCode = 208},
            new CurrencyIso4217 { CurrencyName = "Djibouti Franc", AlphabeticCode = "DJF"   , NumericCode = 262},
            new CurrencyIso4217 { CurrencyName = "Dominican Peso", AlphabeticCode = "DOP"   , NumericCode = 214},
            new CurrencyIso4217 { CurrencyName = "Egyptian Pound", AlphabeticCode = "EGP"   , NumericCode = 818},
            new CurrencyIso4217 { CurrencyName = "El Salvador Colon", AlphabeticCode = "SVC"    , NumericCode = 222},
            new CurrencyIso4217 { CurrencyName = "Nakfa", AlphabeticCode = "ERN"    , NumericCode = 232},
            new CurrencyIso4217 { CurrencyName = "Ethiopian Birr", AlphabeticCode = "ETB"   , NumericCode = 230},
            new CurrencyIso4217 { CurrencyName = "Falkland Islands Pound", AlphabeticCode = "FKP"   , NumericCode = 238},
            new CurrencyIso4217 { CurrencyName = "Fiji Dollar", AlphabeticCode = "FJD"  , NumericCode = 242},
            new CurrencyIso4217 { CurrencyName = "CFP Franc", AlphabeticCode = "XPF"    , NumericCode = 953},
            new CurrencyIso4217 { CurrencyName = "Dalasi", AlphabeticCode = "GMD"   , NumericCode = 270},
            new CurrencyIso4217 { CurrencyName = "Lari", AlphabeticCode = "GEL" , NumericCode = 981},
            new CurrencyIso4217 { CurrencyName = "Ghana Cedi", AlphabeticCode = "GHS"   , NumericCode = 936},
            new CurrencyIso4217 { CurrencyName = "Gibraltar Pound", AlphabeticCode = "GIP"  , NumericCode = 292},
            new CurrencyIso4217 { CurrencyName = "Quetzal", AlphabeticCode = "GTQ"  , NumericCode = 320},
            new CurrencyIso4217 { CurrencyName = "Pound Sterling", AlphabeticCode = "GBP"   , NumericCode = 826},
            new CurrencyIso4217 { CurrencyName = "Guinean Franc", AlphabeticCode = "GNF"    , NumericCode = 324},
            new CurrencyIso4217 { CurrencyName = "Guyana Dollar", AlphabeticCode = "GYD"    , NumericCode = 328},
            new CurrencyIso4217 { CurrencyName = "Gourde", AlphabeticCode = "HTG"   , NumericCode = 332},
            new CurrencyIso4217 { CurrencyName = "Lempira", AlphabeticCode = "HNL"  , NumericCode = 340},
            new CurrencyIso4217 { CurrencyName = "Hong Kong Dollar", AlphabeticCode = "HKD" , NumericCode = 344},
            new CurrencyIso4217 { CurrencyName = "Forint", AlphabeticCode = "HUF"   , NumericCode = 348},
            new CurrencyIso4217 { CurrencyName = "Iceland Krona", AlphabeticCode = "ISK"    , NumericCode = 352},
            new CurrencyIso4217 { CurrencyName = "Rupiah", AlphabeticCode = "IDR"   , NumericCode = 360},
            new CurrencyIso4217 { CurrencyName = "SDR (Special Drawing Right)", AlphabeticCode = "XDR"  , NumericCode = 960},
            new CurrencyIso4217 { CurrencyName = "Iranian Rial", AlphabeticCode = "IRR" , NumericCode = 364},
            new CurrencyIso4217 { CurrencyName = "Iraqi Dinar", AlphabeticCode = "IQD"  , NumericCode = 368},
            new CurrencyIso4217 { CurrencyName = "New Israeli Sheqel", AlphabeticCode = "ILS"   , NumericCode = 376},
            new CurrencyIso4217 { CurrencyName = "Jamaican Dollar", AlphabeticCode = "JMD"  , NumericCode = 388},
            new CurrencyIso4217 { CurrencyName = "Yen", AlphabeticCode = "JPY"  , NumericCode = 392},
            new CurrencyIso4217 { CurrencyName = "Jordanian Dinar", AlphabeticCode = "JOD"  , NumericCode = 400},
            new CurrencyIso4217 { CurrencyName = "Tenge", AlphabeticCode = "KZT"    , NumericCode = 398},
            new CurrencyIso4217 { CurrencyName = "Kenyan Shilling", AlphabeticCode = "KES"  , NumericCode = 404},
            new CurrencyIso4217 { CurrencyName = "North Korean Won", AlphabeticCode = "KPW" , NumericCode = 408},
            new CurrencyIso4217 { CurrencyName = "Won", AlphabeticCode = "KRW"  , NumericCode = 410},
            new CurrencyIso4217 { CurrencyName = "Kuwaiti Dinar", AlphabeticCode = "KWD"    , NumericCode = 414},
            new CurrencyIso4217 { CurrencyName = "Som", AlphabeticCode = "KGS"  , NumericCode = 417},
            new CurrencyIso4217 { CurrencyName = "Lao Kip", AlphabeticCode = "LAK"  , NumericCode = 418},
            new CurrencyIso4217 { CurrencyName = "Lebanese Pound", AlphabeticCode = "LBP"   , NumericCode = 422},
            new CurrencyIso4217 { CurrencyName = "Loti", AlphabeticCode = "LSL" , NumericCode = 426},
            new CurrencyIso4217 { CurrencyName = "Rand", AlphabeticCode = "ZAR" , NumericCode = 710},
            new CurrencyIso4217 { CurrencyName = "Liberian Dollar", AlphabeticCode = "LRD"  , NumericCode = 430},
            new CurrencyIso4217 { CurrencyName = "Libyan Dinar", AlphabeticCode = "LYD" , NumericCode = 434},
            new CurrencyIso4217 { CurrencyName = "Swiss Franc", AlphabeticCode = "CHF"  , NumericCode = 756},
            new CurrencyIso4217 { CurrencyName = "Pataca", AlphabeticCode = "MOP"   , NumericCode = 446},
            new CurrencyIso4217 { CurrencyName = "Denar", AlphabeticCode = "MKD"    , NumericCode = 807},
            new CurrencyIso4217 { CurrencyName = "Malagasy Ariary", AlphabeticCode = "MGA"  , NumericCode = 969},
            new CurrencyIso4217 { CurrencyName = "Malawi Kwacha", AlphabeticCode = "MWK"    , NumericCode = 454},
            new CurrencyIso4217 { CurrencyName = "Malaysian Ringgit", AlphabeticCode = "MYR"    , NumericCode = 458},
            new CurrencyIso4217 { CurrencyName = "Rufiyaa", AlphabeticCode = "MVR"  , NumericCode = 462},
            new CurrencyIso4217 { CurrencyName = "Ouguiya", AlphabeticCode = "MRU"  , NumericCode = 929},
            new CurrencyIso4217 { CurrencyName = "Mauritius Rupee", AlphabeticCode = "MUR"  , NumericCode = 480},
            new CurrencyIso4217 { CurrencyName = "ADB Unit of Account", AlphabeticCode = "XUA"  , NumericCode = 965},
            new CurrencyIso4217 { CurrencyName = "Mexican Peso", AlphabeticCode = "MXN" , NumericCode = 484},
            new CurrencyIso4217 { CurrencyName = "Mexican Unidad de Inversion (UDI)", AlphabeticCode = "MXV"    , NumericCode = 979},
            new CurrencyIso4217 { CurrencyName = "Moldovan Leu", AlphabeticCode = "MDL" , NumericCode = 498},
            new CurrencyIso4217 { CurrencyName = "Tugrik", AlphabeticCode = "MNT"   , NumericCode = 496},
            new CurrencyIso4217 { CurrencyName = "Moroccan Dirham", AlphabeticCode = "MAD"  , NumericCode = 504},
            new CurrencyIso4217 { CurrencyName = "Mozambique Metical", AlphabeticCode = "MZN"   , NumericCode = 943},
            new CurrencyIso4217 { CurrencyName = "Kyat", AlphabeticCode = "MMK" , NumericCode = 104},
            new CurrencyIso4217 { CurrencyName = "Namibia Dollar", AlphabeticCode = "NAD"   , NumericCode = 516},
            new CurrencyIso4217 { CurrencyName = "Nepalese Rupee", AlphabeticCode = "NPR"   , NumericCode = 524},
            new CurrencyIso4217 { CurrencyName = "Cordoba Oro", AlphabeticCode = "NIO"  , NumericCode = 558},
            new CurrencyIso4217 { CurrencyName = "Naira", AlphabeticCode = "NGN"    , NumericCode = 566},
            new CurrencyIso4217 { CurrencyName = "Rial Omani", AlphabeticCode = "OMR"   , NumericCode = 512},
            new CurrencyIso4217 { CurrencyName = "Pakistan Rupee", AlphabeticCode = "PKR"   , NumericCode = 586},
            new CurrencyIso4217 { CurrencyName = "Balboa", AlphabeticCode = "PAB"   , NumericCode = 590},
            new CurrencyIso4217 { CurrencyName = "Kina", AlphabeticCode = "PGK" , NumericCode = 598},
            new CurrencyIso4217 { CurrencyName = "Guarani", AlphabeticCode = "PYG"  , NumericCode = 600},
            new CurrencyIso4217 { CurrencyName = "Sol", AlphabeticCode = "PEN"  , NumericCode = 604},
            new CurrencyIso4217 { CurrencyName = "Philippine Piso", AlphabeticCode = "PHP"  , NumericCode = 608},
            new CurrencyIso4217 { CurrencyName = "Zloty", AlphabeticCode = "PLN"    , NumericCode = 985},
            new CurrencyIso4217 { CurrencyName = "Qatari Rial", AlphabeticCode = "QAR"  , NumericCode = 634},
            new CurrencyIso4217 { CurrencyName = "Romanian Leu", AlphabeticCode = "RON" , NumericCode = 946},
            new CurrencyIso4217 { CurrencyName = "Russian Ruble", AlphabeticCode = "RUB"    , NumericCode = 643},
            new CurrencyIso4217 { CurrencyName = "Rwanda Franc", AlphabeticCode = "RWF" , NumericCode = 646},
            new CurrencyIso4217 { CurrencyName = "Saint Helena Pound", AlphabeticCode = "SHP"   , NumericCode = 654},
            new CurrencyIso4217 { CurrencyName = "Tala", AlphabeticCode = "WST" , NumericCode = 882},
            new CurrencyIso4217 { CurrencyName = "Dobra", AlphabeticCode = "STN"    , NumericCode = 930},
            new CurrencyIso4217 { CurrencyName = "Saudi Riyal", AlphabeticCode = "SAR"  , NumericCode = 682},
            new CurrencyIso4217 { CurrencyName = "Serbian Dinar", AlphabeticCode = "RSD"    , NumericCode = 941},
            new CurrencyIso4217 { CurrencyName = "Seychelles Rupee", AlphabeticCode = "SCR" , NumericCode = 690},
            new CurrencyIso4217 { CurrencyName = "Leone", AlphabeticCode = "SLL"    , NumericCode = 694},
            new CurrencyIso4217 { CurrencyName = "Singapore Dollar", AlphabeticCode = "SGD" , NumericCode = 702},
            new CurrencyIso4217 { CurrencyName = "Sucre", AlphabeticCode = "XSU"    , NumericCode = 994},
            new CurrencyIso4217 { CurrencyName = "Solomon Islands Dollar", AlphabeticCode = "SBD"   , NumericCode = 090},
            new CurrencyIso4217 { CurrencyName = "Somali Shilling", AlphabeticCode = "SOS"  , NumericCode = 706},
            new CurrencyIso4217 { CurrencyName = "South Sudanese Pound", AlphabeticCode = "SSP" , NumericCode = 728},
            new CurrencyIso4217 { CurrencyName = "Sri Lanka Rupee", AlphabeticCode = "LKR"  , NumericCode = 144},
            new CurrencyIso4217 { CurrencyName = "Sudanese Pound", AlphabeticCode = "SDG"   , NumericCode = 938},
            new CurrencyIso4217 { CurrencyName = "Surinam Dollar", AlphabeticCode = "SRD"   , NumericCode = 968},
            new CurrencyIso4217 { CurrencyName = "Lilangeni", AlphabeticCode = "SZL"    , NumericCode = 748},
            new CurrencyIso4217 { CurrencyName = "Swedish Krona", AlphabeticCode = "SEK"    , NumericCode = 752},
            new CurrencyIso4217 { CurrencyName = "WIR Euro", AlphabeticCode = "CHE" , NumericCode = 947},
            new CurrencyIso4217 { CurrencyName = "WIR Franc", AlphabeticCode = "CHW"    , NumericCode = 948},
            new CurrencyIso4217 { CurrencyName = "Syrian Pound", AlphabeticCode = "SYP" , NumericCode = 760},
            new CurrencyIso4217 { CurrencyName = "New Taiwan Dollar", AlphabeticCode = "TWD"    , NumericCode = 901},
            new CurrencyIso4217 { CurrencyName = "Somoni", AlphabeticCode = "TJS"   , NumericCode = 972},
            new CurrencyIso4217 { CurrencyName = "Tanzanian Shilling", AlphabeticCode = "TZS"   , NumericCode = 834},
            new CurrencyIso4217 { CurrencyName = "Baht", AlphabeticCode = "THB" , NumericCode = 764},
            new CurrencyIso4217 { CurrencyName = "Pa’anga", AlphabeticCode = "TOP"  , NumericCode = 776},
            new CurrencyIso4217 { CurrencyName = "Trinidad and Tobago Dollar", AlphabeticCode = "TTD"   , NumericCode = 780},
            new CurrencyIso4217 { CurrencyName = "Tunisian Dinar", AlphabeticCode = "TND"   , NumericCode = 788},
            new CurrencyIso4217 { CurrencyName = "Turkish Lira", AlphabeticCode = "TRY" , NumericCode = 949},
            new CurrencyIso4217 { CurrencyName = "Turkmenistan New Manat", AlphabeticCode = "TMT"   , NumericCode = 934},
            new CurrencyIso4217 { CurrencyName = "Uganda Shilling", AlphabeticCode = "UGX"  , NumericCode = 800},
            new CurrencyIso4217 { CurrencyName = "Hryvnia", AlphabeticCode = "UAH"  , NumericCode = 980},
            new CurrencyIso4217 { CurrencyName = "UAE Dirham", AlphabeticCode = "AED"   , NumericCode = 784},
            new CurrencyIso4217 { CurrencyName = "US Dollar (Next day)", AlphabeticCode = "USN" , NumericCode = 997},
            new CurrencyIso4217 { CurrencyName = "Peso Uruguayo", AlphabeticCode = "UYU"    , NumericCode = 858},
            new CurrencyIso4217 { CurrencyName = "Uruguay Peso en Unidades Indexadas (URUIURUI)", AlphabeticCode = "UYI", NumericCode = 940},
            new CurrencyIso4217 { CurrencyName = "Uzbekistan Sum", AlphabeticCode = "UZS"   , NumericCode = 860},
            new CurrencyIso4217 { CurrencyName = "Vatu", AlphabeticCode = "VUV" , NumericCode = 548},
            new CurrencyIso4217 { CurrencyName = "Bolívar", AlphabeticCode = "VEF"  , NumericCode = 937},
            new CurrencyIso4217 { CurrencyName = "Dong", AlphabeticCode = "VND" , NumericCode = 704},
            new CurrencyIso4217 { CurrencyName = "Yemeni Rial", AlphabeticCode = "YER"  , NumericCode = 886},
            new CurrencyIso4217 { CurrencyName = "Zambian Kwacha", AlphabeticCode = "ZMW"   , NumericCode = 967},
            new CurrencyIso4217 { CurrencyName = "Zimbabwe Dollar"  , AlphabeticCode = "ZWL", NumericCode = 932}
        };

        /// <summary>
        /// Get currency by name
        /// </summary>
        /// <param name="currencyName">currency name</param>
        /// <returns>The matching currency or null</returns>
        public virtual CurrencyIso4217 GetCurrencyByCurrencyName(string currencyName)
        {
            var uppered = currencyName.ToUpper();

            return currencies.SingleOrDefault(x => x.CurrencyName.ToUpper() == uppered);
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
