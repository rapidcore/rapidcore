
namespace RapidCore.Globalization
{
    /// <summary>
    /// Defines a currency based on https://www.iso.org/iso-4217-currency-codes.html
    /// </summary>
    public class CurrencyIso4217
    {
        /// <summary>
        /// The english name of the currency
        /// </summary>
        public virtual string NameEnglish { get; set; }
        
        /// <summary>
        /// The currency's alphabetic code, i.e. USD
        /// </summary>
        public virtual string CodeAlpha { get; set; }
        
        /// <summary>
        /// The currency's numeric code, i.e. 840
        /// </summary>
        public virtual int CodeNumeric { get; set; }
        
        /// <summary>
        /// How many decimal places this currency uses
        /// </summary>
        public virtual int MinorUnit { get; set; }
        
        /// <summary>
        /// Does this currency have the given currency code.
        ///
        /// If given <c>null</c>, <c>false</c> is returned.
        /// </summary>
        /// <param name="alphaOrNumeric">Currency code to check as either alpha or numeric (null is handled)</param>
        public virtual bool Is(string alphaOrNumeric)
        {
            if (string.IsNullOrEmpty(alphaOrNumeric))
            {
                return false;
            }
            
            if (int.TryParse(alphaOrNumeric, out int numeric))
            {
                return numeric == CodeNumeric;
            }

            return CodeAlpha.Equals(alphaOrNumeric.ToUpper());
        }
    }
}
