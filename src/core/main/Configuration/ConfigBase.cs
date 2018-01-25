#if NETSTANDARD1_6

using System;
using Microsoft.Extensions.Configuration;

namespace RapidCore.Configuration
{
    /// <summary>
    /// Base class for strongly typed configuration classes.
    ///
    /// The idea is to allow you to define a config class specific
    /// for your project, but gain easy access to reading config values
    /// from wherever.
    /// </summary>
    public abstract class ConfigBase
    {
        private readonly IConfigurationRoot configuration;

        public ConfigBase(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        protected T Get<T>(string key, T defaultValue)
        {
            string value = configuration[key];

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
#endif