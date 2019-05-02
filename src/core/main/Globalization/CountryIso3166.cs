namespace RapidCore.Globalization
{
    /// <summary>
    /// Defines a country based on https://en.wikipedia.org/wiki/ISO_3166-1
    /// </summary>
    public class CountryIso3166
    {
        /// <summary>
        /// The two-letter country code (ISO 3166-1 alpha 2)
        /// https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2
        /// </summary>
        public virtual string CodeAlpha2 { get; set; }

        /// <summary>
        /// The three-letter country code (ISO 3166-1 alpha 3)
        /// https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3
        /// </summary>
        public virtual string CodeAlpha3 { get; set; }

        /// <summary>
        /// The numeric country code (ISO 3166-1 numeric)
        /// https://en.wikipedia.org/wiki/ISO_3166-1_numeric
        /// </summary>
        public virtual int CodeNumeric { get; set; }

        /// <summary>
        /// The english name of the country
        /// </summary>
        public virtual string NameEnglish { get; set; }

        /// <summary>
        /// Does this country have the given country code.
        ///
        /// If given <c>null</c>, <c>false</c> is returned.
        /// </summary>
        /// <param name="alpha2OrAlpha3OrNumeric">Country code to check as either alpha-2, alpha-3 or numeric (null is handled)</param>
        public virtual bool Is(string alpha2OrAlpha3OrNumeric)
        {
            if (string.IsNullOrEmpty(alpha2OrAlpha3OrNumeric))
            {
                return false;
            }
            
            if (int.TryParse(alpha2OrAlpha3OrNumeric, out int numeric))
            {
                return numeric == CodeNumeric;
            }

            var uppered = alpha2OrAlpha3OrNumeric.ToUpper();

            return CodeAlpha2.Equals(uppered) || CodeAlpha3.Equals(uppered);
        }
    }
}