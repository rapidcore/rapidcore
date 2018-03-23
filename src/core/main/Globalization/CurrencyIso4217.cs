using System;
using System.Collections.Generic;
using System.Text;

namespace RapidCore.Globalization
{
    /// <summary>
    /// Defines a currency based on https://en.wikipedia.org/wiki/ISO_4217
    /// </summary>
    public class CurrencyIso4217
    {
        /// <summary>
        /// The currency name
        /// </summary>
        public virtual string CurrencyName { get; set; }
        /// <summary>
        /// The currency's alphabetic code, i.e. USD
        /// </summary>
        public virtual string AlphabeticCode { get; set; }
        /// <summary>
        /// The currency's numeric code, i.e. 840
        /// </summary>
        public virtual int NumericCode { get; set; }
    }
}
