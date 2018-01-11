using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using RapidCore.Configuration;
using Xunit;

namespace RapidCore.UnitTests.Configuration
{
    public class ConfigBaseTests
    {
        private MyTestConfig config;

        public ConfigBaseTests()
        {
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "string", "from config" },
                    { "string_null", null },
                    { "string_empty", string.Empty },
                    { "int", "3" },
                    { "int_zero", "0" }
                });

            config = new MyTestConfig(builder.Build());
        }

        [Fact]
        public void Get_String()
        {
            Assert.Equal("from config", config.Get<string>("string", "default"));
        }

        [Fact]
        public void Get_String_Default_IfNull()
        {
            Assert.Equal("default", config.Get<string>("string_null", "default"));
        }

        [Fact]
        public void Get_String_Default_IfEmpty()
        {
            Assert.Equal("default", config.Get<string>("string_empty", "default"));
        }

        [Fact]
        public void Get_String_Default_IfUndefined()
        {
            Assert.Equal("default", config.Get<string>("does_not_exist_coz_that_would_be_weird", "default"));
        }

        [Fact]
        public void Get_Int()
        {
            Assert.Equal(3, config.Get<int>("int", 999));
        }

        [Fact]
        public void Get_Int_zeroIsValid()
        {
            Assert.Equal(0, config.Get<int>("int_zero", 999));
        }

        [Fact]
        public void Get_Int_Default_IfUndefined()
        {
            Assert.Equal(999, config.Get<int>("does_not_exist_coz_that_would_be_weird", 999));
        }

        #region Config implementation
        private class MyTestConfig : ConfigBase
        {
            public MyTestConfig(IConfigurationRoot configuration) : base(configuration) { }

            public new T Get<T>(string key, T defaultValue)
            {
                return base.Get<T>(key, defaultValue);
            }
        }
        #endregion
    }
}