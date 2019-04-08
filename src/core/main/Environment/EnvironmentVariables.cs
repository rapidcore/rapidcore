using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace RapidCore.Environment
{
    /// <summary>
    /// Convenient and mockable methods
    /// for working with environment variables.
    /// </summary>
    public class EnvironmentVariables
    {
        /// <summary>
        /// Get the value of an environment variable. If the value
        /// does not exist or is empty, the <paramref name="defaultValue"/> will
        /// be returned instead.
        /// </summary>
        /// <param name="key">The key/name of the variable</param>
        /// <param name="defaultValue">The value to use if the environment does not have one</param>
        /// <returns>The environment's value for <paramref name="key"/> or <paramref name="defaultValue"/></returns>
        public virtual T Get<T>(string key, T defaultValue)
        {
            var value = System.Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            var converted = TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);

            if (converted == null)
            {
                return defaultValue;
            }
            
            return (T) converted;
        }

        /// <summary>
        /// Get _all_ environment variables ordered by name
        /// </summary>
        /// <returns>A sorted dictionary with _all_ defined environment variables</returns>
        public virtual SortedDictionary<string, string> AllSorted()
        {
            var idictionary = System.Environment.GetEnvironmentVariables();
            var sorted = new SortedDictionary<string, string>();

            foreach (DictionaryEntry de in idictionary)
            {
                sorted.Add(de.Key.ToString(), de.Value.ToString());
            }

            return sorted;
        }
    }
}