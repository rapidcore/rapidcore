
namespace RapidCore.Globalization
{
    /// <summary>
    /// Defines a currency based on https://www.iso.org/iso-4217-currency-codes.html
    /// </summary>
    public class CurrencyIso4217
    {
        /// <summary>
        /// The currency name
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// The currency's alphabetic code, i.e. USD
        /// </summary>
        public virtual string AlphabeticCode { get; set; }
        
        /// <summary>
        /// The currency's numeric code, i.e. 840
        /// </summary>
        public virtual int NumericCode { get; set; }
        
        /// <summary>
        /// How many decimal places this currency uses
        /// </summary>
        public virtual int MinorUnit { get; set; }
    }
}
