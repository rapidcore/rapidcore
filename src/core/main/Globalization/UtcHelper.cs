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
    }
}