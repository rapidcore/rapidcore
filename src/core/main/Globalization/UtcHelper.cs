using System;

namespace RapidCore.Globalization
{
    /// <summary>
    /// Knows how to handle UTC.
    /// </summary>
    public class UtcHelper
    {
        /// <summary>
        /// Get the current date and time in UTC
        /// </summary>
        public virtual DateTime Now()
        {
            return DateTime.UtcNow;
        }

        /// <summary>
        /// Convert the given datetime to UTC. If it
        /// is already UTC, then no harm is done.
        /// </summary>
        /// <param name="possiblyNonUtc">A DateTime that may or may not be in UTC already</param>
        /// <returns>The given DateTime converted to UTC</returns>
        public virtual DateTime ToUtc(DateTime possiblyNonUtc)
        {
            return possiblyNonUtc.ToUniversalTime();
        }

        /// <summary>
        /// Convert a given string into a UTC DateTime instance.
        /// 
        /// It accepts whatever DateTime.Ctor(..) accepts.
        /// </summary>
        /// <param name="whateverDateTimeAccepts">Whatever string the DateTime constructor accepts</param>
        /// <returns>A UTC DateTime</returns>
        public virtual DateTime ToUtc(string whateverDateTimeAccepts)
        {
            return DateTime.Parse(whateverDateTimeAccepts).ToUniversalTime();
        }
    }
}