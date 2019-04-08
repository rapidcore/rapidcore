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
            var value = configuration[key];

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
    }
}
