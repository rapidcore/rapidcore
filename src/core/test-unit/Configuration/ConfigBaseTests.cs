#if NETSTANDARD1_6
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
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
                    { "int_zero", "0" },
                    { "section:section-1", "s1 from config" },
                    { "section:subsection:subsection-1", "subsection 1 from config"},
                    { "enum", "One" },
                    { "enum_null", null },
                    { "enum_empty", string.Empty },
                    { "enum_invalid", "NotAValidValue" }
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
        
        [Fact]
        public void Section_syntax_is_correct()
        {
            // tests that the "parsed" json section
            // syntax used in the contructor of this class
            // is correct

            var actual = config.GetSection("section");
            
            Assert.Equal("s1 from config", actual["section-1"]);

            var subsection = actual.GetSection("subsection");
            
            Assert.Equal("subsection 1 from config", subsection["subsection-1"]);
        }
        
        [Fact]
        public void Get_Section_exists_key_exists()
        {
            Assert.Equal("s1 from config", config.Get<string>("section:section-1", "default"));
        }
        
        [Fact]
        public void Get_Section_if_section_is_undefined_return_default()
        {
            Assert.Equal("default", config.Get<string>("no-such-section:yummy", "default"));
        }
        
        [Fact]
        public void Get_Section_if_key_is_undefined_return_default()
        {
            Assert.Equal("default", config.Get<string>("section:yummy", "default"));
        }
        
        [Fact]
        public void Get_SubSection_exists_key_exists()
        {
            Assert.Equal("subsection 1 from config", config.Get<string>("section:subsection:subsection-1", "default"));
        }

        [Fact]
        public void Get_Enum()
        {
            Assert.Equal(MyTestConfigThing.One, config.Get<MyTestConfigThing>("enum", MyTestConfigThing.Zero));
        }

        [Fact]
        public void Get_Enum_Default_IfNull()
        {
            Assert.Equal(MyTestConfigThing.One, config.Get<MyTestConfigThing>("enum_null", MyTestConfigThing.One));
        }

        [Fact]
        public void Get_Enum_Default_IfEmpty()
        {
            Assert.Equal(MyTestConfigThing.One, config.Get<MyTestConfigThing>("enum_empty", MyTestConfigThing.One));
        }

        [Fact]
        public void Get_Enum_Default_IfUndefined()
        {
            Assert.Equal(MyTestConfigThing.One, config.Get<MyTestConfigThing>("does_not_exist_coz_that_would_be_weird", MyTestConfigThing.One));
        }
        
        [Fact]
        public void Get_Enum_Default_IfInvalidValue()
        {
            Assert.Equal(MyTestConfigThing.One, config.Get<MyTestConfigThing>("enum_invalid", MyTestConfigThing.One));
        }
        
        

        #region Config implementation
        private class MyTestConfig : ConfigBase
        {
            private readonly IConfigurationRoot configuration;
            
            public MyTestConfig(IConfigurationRoot configuration) : base(configuration)
            {
                this.configuration = configuration;
            }

            public new T Get<T>(string key, T defaultValue)
            {
                return base.Get<T>(key, defaultValue);
            }

            public IConfigurationSection GetSection(string key)
            {
                return configuration.GetSection(key);
            }
        }

        private enum MyTestConfigThing
        {
            Zero = 0,
            One = 1
        }
        #endregion
    }
}
#endif