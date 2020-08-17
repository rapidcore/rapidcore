using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace RapidCore.Configuration
{
    /// <summary>
    /// Extensions for _getting_ values from <see cref="IConfiguration"/>
    /// </summary>
    public static class ConfigurationGetExtensions
    {
        /// <summary>
        /// Get a specific configuration key value - or default.
        /// </summary>
        public static T Get<T>(this IConfiguration config, string key, T defaultValue)
        {
            var value = config[key];

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            if (TypeDescriptor.GetConverter(typeof(T)).IsValid(value))
            {
                return (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
            }
            return defaultValue;
        }
    }
}