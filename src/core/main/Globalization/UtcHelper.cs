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
    }
}