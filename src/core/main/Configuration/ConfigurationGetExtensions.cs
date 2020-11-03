using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace RapidCore.Configuration
{
    /// <summary>
    /// Extensions for _getting_ values from <see cref="IConfiguration"/>
    /// </summary>
    public static class ConfigurationGetExtensions
    {
        private static T ConvertOrDefault<T>(string value, T defaultValue)
        {
            if (TypeDescriptor.GetConverter(typeof(T)).IsValid(value))
            {
                return (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
            }
            return defaultValue;
        }
        
        /// <summary>
        /// Get a specific configuration key value - or default.
        /// </summary>
        public static T Get<T>(this IConfiguration config, string key, T defaultValue)
        {
            /*
             * This method does not call the multiKey version, as that
             * would require creating an array that we do not really need.
             * Since the 1-key version is likely to be the most heavily used,
             * we might as well make it the cheapest version.
             */
            var value = config[key];

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return ConvertOrDefault(value, defaultValue);
        }

        /// <summary>
        /// Get a configuration value that can be behind multiple keys - or default.
        /// The keys are checked _in order_.
        ///
        /// This is a convenience overload for Get(string[] keys, T defaultValue).
        /// </summary>
        public static T Get<T>(this IConfiguration config, string keyPrimary, string keySecondary, T defaultValue)
        {
            return config.Get(new[] {keyPrimary, keySecondary}, defaultValue);
        }
        
        /// <summary>
        /// Get a configuration value that can be behind multiple keys - or default.
        /// The keys are checked _in order_.
        ///
        /// This is a convenience overload for Get(string[] keys, T defaultValue).
        /// </summary>
        public static T Get<T>(this IConfiguration config, string keyPrimary, string keySecondary, string keyTertiary, T defaultValue)
        {
            return config.Get(new[] {keyPrimary, keySecondary, keyTertiary}, defaultValue);
        }

        /// <summary>
        /// Get a configuration value that can be behind multiple keys - or default.
        /// The keys are checked _in order_.
        ///
        /// The idea behind this, is that you might have a scenario where you have multiple
        /// specific things that might share a common config value.
        ///
        /// Example:
        /// Multiple databases in code, but actually running on the same host. In this case, you
        /// probably want to avoid having to duplicate the hostname to every single database config,
        /// but you want to retain the option of each of them overriding the "shared" value.
        ///
        /// Instead of the following, where there is no sharing...
        /// {
        ///   "db_a": { "host": "shared_host", "name": "a" },
        ///   "db_b": { "host": "shared_host", "name": "b" }
        /// }
        ///
        /// You could now have...
        ///
        /// {
        ///   "db_shared": { "host": "shared_host" },
        ///   "db_a": { "name": "a" },
        ///   "db_b": { "name": "b" }
        /// }
        ///
        /// And then use:
        /// config.Get("db_a:host", "db_shared:host", "defaultValue");
        /// </summary>
        public static T Get<T>(this IConfiguration config, IEnumerable<string> keys, T defaultValue)
        {
            foreach (var key in keys)
            {
                var value = config[key];

                if (!string.IsNullOrEmpty(value))
                {
                    return ConvertOrDefault(value, defaultValue);
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// This is a convenience overload for GetFromCommaSeparatedList(string[] keys, List<T> defaultValue)
        /// </summary>
        public static List<T> GetFromCommaSeparatedList<T>(this IConfiguration config, string key, List<T> defaultValue)
        {
            var value = config.Get<string>(key, null);

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return value
                .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => ConvertOrDefault(x.Trim(), default(T)))
                .ToList();
        }
        
        /// <summary>
        /// This is a convenience overload for GetFromCommaSeparatedList(string[] keys, List<T> defaultValue)
        /// </summary>
        public static List<T> GetFromCommaSeparatedList<T>(this IConfiguration config, string keyPrimary, string keySecondary, List<T> defaultValue)
        {
            return config.GetFromCommaSeparatedList(new[] {keyPrimary, keySecondary}, defaultValue);
        }
        
        /// <summary>
        /// This is a convenience overload for GetFromCommaSeparatedList(string[] keys, List<T> defaultValue)
        /// </summary>
        public static List<T> GetFromCommaSeparatedList<T>(this IConfiguration config, string keyPrimary, string keySecondary, string keyTertiary, List<T> defaultValue)
        {
            return config.GetFromCommaSeparatedList(new[] {keyPrimary, keySecondary, keyTertiary}, defaultValue);
        }
        
        
        /// <summary>
        /// Treat a value as a comma separated list of values of some type - or return a default value.
        /// The raw value is determined using the Get extension method.
        ///
        /// The parsing is as follows:
        /// 1. the raw value is split by "comma"
        /// 2. empty values are removed
        /// 3. each value is trimmed and converted (just like Get would do)
        /// </summary>
        public static List<T> GetFromCommaSeparatedList<T>(this IConfiguration config, IEnumerable<string> keys, List<T> defaultValue)
        {
            var value = config.Get<string>(keys, null);

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return value
                .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => ConvertOrDefault(x.Trim(), default(T)))
                .ToList();
        }
    }
}