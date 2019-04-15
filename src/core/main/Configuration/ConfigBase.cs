#if NETSTANDARD2_0
using System;
using System.Reflection;
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
        private readonly IConfiguration configuration;

        protected ConfigBase(IConfiguration configuration)
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

            if (typeof(T).GetTypeInfo().IsEnum)
            {
                // try..catch is not as pretty as Enum.TryParse, but
                // TryParse requires T to be non-nullable, which is
                // not a constraint for us
                try
                {
                    return (T)Enum.Parse(typeof(T), value, true);
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
#endif