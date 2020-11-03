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
    [Obsolete("No longer necessary. The methods have been implemented as extension methods on Microsoft.Extensions.Configuration.IConfiguration")]
    public abstract class ConfigBase
    {
        private readonly IConfiguration configuration;

        protected ConfigBase(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected T Get<T>(string key, T defaultValue)
        {
            return configuration.Get(key, defaultValue);
        }
    }
}