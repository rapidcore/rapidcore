using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using RapidCore.Configuration;
using Xunit;

namespace UnitTests.Core.Configuration
{
    public class ConfigurationGetExtensionsTests
    {
        private readonly IConfiguration config;

        public ConfigurationGetExtensionsTests()
        {
            config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "string", "from config" },
                    { "string_null", null },
                    { "string_empty", string.Empty },
                    { "string_whitespace", "   "},
                    { "int", "3" },
                    { "int_zero", "0" },
                    { "section:section-1", "s1 from config" },
                    { "section:subsection:subsection-1", "subsection 1 from config"},
                    { "enum", "One" },
                    { "enum_null", null },
                    { "enum_empty", string.Empty },
                    { "enum_invalid", "NotAValidValue" }
                }).Build();
        }
        
        #region Single key getter
        [Fact]
        public void Get_singleKey_String()
        {
            Assert.Equal("from config", config.Get<string>("string", "default"));
        }
        
        [Fact]
        public void Get_singleKey_String_whitespaceIsConsideredAValue()
        {
            Assert.Equal("   ", config.Get<string>("string_whitespace", "default"));
        }

        [Fact]
        public void Get_singleKey_String_Default_IfNull()
        {
            Assert.Equal("default", config.Get<string>("string_null", "default"));
        }

        [Fact]
        public void Get_singleKey_String_Default_IfEmpty()
        {
            Assert.Equal("default", config.Get<string>("string_empty", "default"));
        }

        [Fact]
        public void Get_singleKey_String_Default_IfUndefined()
        {
            Assert.Equal("default", config.Get<string>("does_not_exist_coz_that_would_be_weird", "default"));
        }

        [Fact]
        public void Get_singleKey_Int()
        {
            Assert.Equal(3, config.Get<int>("int", 999));
        }

        [Fact]
        public void Get_singleKey_Int_zeroIsValid()
        {
            Assert.Equal(0, config.Get<int>("int_zero", 999));
        }

        [Fact]
        public void Get_singleKey_Int_Default_IfUndefined()
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
        public void Get_singleKey_Section_exists_key_exists()
        {
            Assert.Equal("s1 from config", config.Get<string>("section:section-1", "default"));
        }
        
        [Fact]
        public void Get_singleKey_Section_if_section_is_undefined_return_default()
        {
            Assert.Equal("default", config.Get<string>("no-such-section:yummy", "default"));
        }
        
        [Fact]
        public void Get_singleKey_Section_if_key_is_undefined_return_default()
        {
            Assert.Equal("default", config.Get<string>("section:yummy", "default"));
        }
        
        [Fact]
        public void Get_singleKey_SubSection_exists_key_exists()
        {
            Assert.Equal("subsection 1 from config", config.Get<string>("section:subsection:subsection-1", "default"));
        }

        [Fact]
        public void Get_singleKey_Enum()
        {
            Assert.Equal(MyTestConfigThing.One, config.Get<MyTestConfigThing>("enum", MyTestConfigThing.Zero));
        }

        [Fact]
        public void Get_singleKey_Enum_Default_IfNull()
        {
            Assert.Equal(MyTestConfigThing.One, config.Get<MyTestConfigThing>("enum_null", MyTestConfigThing.One));
        }

        [Fact]
        public void Get_singleKey_Enum_Default_IfEmpty()
        {
            Assert.Equal(MyTestConfigThing.One, config.Get<MyTestConfigThing>("enum_empty", MyTestConfigThing.One));
        }

        [Fact]
        public void Get_singleKey_Enum_Default_IfUndefined()
        {
            Assert.Equal(MyTestConfigThing.One, config.Get<MyTestConfigThing>("does_not_exist_coz_that_would_be_weird", MyTestConfigThing.One));
        }
        
        [Fact]
        public void Get_singleKey_Enum_Default_IfInvalidValue()
        {
            Assert.Equal(MyTestConfigThing.One, config.Get<MyTestConfigThing>("enum_invalid", MyTestConfigThing.One));
        }
        #endregion

        #region multi key getter
        [Theory]
        [InlineData("1", "2", "3", "default", "1")]
        [InlineData("   ", "2", "3", "default", "   ")] // whitespace is a valid value
        // primary not set
        [InlineData(null, "2", "3", "default", "2")]
        [InlineData("", "2", "3", "default", "2")]
        [InlineData(null, "  ", "3", "default", "  ")] // whitespace is a valid value
        // primary and secondary not set
        [InlineData(null, null, "3", "default", "3")]
        [InlineData(null, "", "3", "default", "3")]
        [InlineData(null, null, "   ", "default", "   ")] // whitespace is a valid value
        // no keys set
        [InlineData(null, null, null, "default", "default")]
        [InlineData(null, null, "", "default", "default")]
        public void Get_multiKey_respectsOrdering(string primaryValue, string secondaryValue, string tertiaryValue, string defaultValue, string expected)
        {
            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "primary", primaryValue },
                    { "secondary", secondaryValue },
                    { "tertiary", tertiaryValue }
                }).Build();

            var actual = conf.Get(new[] {"primary", "secondary", "tertiary"}, defaultValue);
            
            Assert.Equal(expected, actual);
        }
        #endregion
        
        #region 2-key getter
        [Theory]
        [InlineData("1", "2", "default", "1")]
        [InlineData("   ", "2", "default", "   ")] // whitespace is a valid value
        // primary not set
        [InlineData(null, "2", "default", "2")]
        [InlineData("", "2", "default", "2")]
        [InlineData(null, "  ", "default", "  ")] // whitespace is a valid value
        // no keys set
        [InlineData(null, null, "default", "default")]
        public void Get_2Key_respectsOrdering(string primaryValue, string secondaryValue, string defaultValue, string expected)
        {
            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "primary", primaryValue },
                    { "secondary", secondaryValue }
                }).Build();

            var actual = conf.Get("primary", "secondary", defaultValue);
            
            Assert.Equal(expected, actual);
        }
        #endregion
        
        #region 3-key getter
        [Theory]
        [InlineData("1", "2", "3", "default", "1")]
        [InlineData("   ", "2", "3", "default", "   ")] // whitespace is a valid value
        // primary not set
        [InlineData(null, "2", "3", "default", "2")]
        [InlineData("", "2", "3", "default", "2")]
        [InlineData(null, "  ", "3", "default", "  ")] // whitespace is a valid value
        // primary and secondary not set
        [InlineData(null, null, "3", "default", "3")]
        [InlineData(null, "", "3", "default", "3")]
        [InlineData(null, null, "   ", "default", "   ")] // whitespace is a valid value
        // no keys set
        [InlineData(null, null, null, "default", "default")]
        [InlineData(null, null, "", "default", "default")]
        public void Get_3Key_respectsOrdering(string primaryValue, string secondaryValue, string tertiaryValue, string defaultValue, string expected)
        {
            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "primary", primaryValue },
                    { "secondary", secondaryValue },
                    { "tertiary", tertiaryValue }
                }).Build();

            var actual = conf.Get("primary", "secondary", "tertiary", defaultValue);
            
            Assert.Equal(expected, actual);
        }
        #endregion

        #region GetCommaSeparatedList (single key)
        [Fact]
        public void GetCommaSeparatedList_returns_default_whenKeyDoesNotExist()
        {
            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "not_this_key", "some value" }
                }).Build();

            var theDefault = new List<string> { "default" };
            
            var actual = conf.GetCommaSeparatedList("pick_me", theDefault);
            
            Assert.Same(actual, theDefault);
            Assert.Single(actual);
            Assert.Equal("default", actual[0]);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GetCommaSeparatedList_returns_default_whenKeyExistsButIsNullOrEmpty(string keyValue)
        {
            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "not_this_key", "some value" },
                    { "pick_me", keyValue }
                }).Build();

            var theDefault = new List<string> { "default" };
            
            var actual = conf.GetCommaSeparatedList("pick_me", theDefault);
            
            Assert.Same(actual, theDefault);
            Assert.Single(actual);
            Assert.Equal("default", actual[0]);
        }
        
        [Fact]
        public void GetCommaSeparatedList_string_returns_trimmedAndSeparated()
        {
            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "not_this_key", "some value" },
                    { "pick_me", ", a   , , b,c,d"}
                }).Build();

            var actual = conf.GetCommaSeparatedList("pick_me", new List<string>(0));
            
            Assert.Equal(4, actual.Count);
            Assert.Equal("a", actual[0]);
            Assert.Equal("b", actual[1]);
            Assert.Equal("c", actual[2]);
            Assert.Equal("d", actual[3]);
        }
        
        [Fact]
        public void GetCommaSeparatedList_int()
        {
            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "not_this_key", "some value" },
                    { "pick_me", ", 1  , , 2,3,4"}
                }).Build();

            var actual = conf.GetCommaSeparatedList("pick_me", new List<int>(0));
            
            Assert.Equal(4, actual.Count);
            Assert.Equal(1, actual[0]);
            Assert.Equal(2, actual[1]);
            Assert.Equal(3, actual[2]);
            Assert.Equal(4, actual[3]);
        }
        #endregion

        #region GetCommaSeparatedList (multi key)
        [Theory]
        [InlineData("1", "2", "default", "1")]
        [InlineData("   ", "2", "default", "--empty--")] // whitespace is a valid value
        // primary not set
        [InlineData(null, "2", "default", "2")]
        [InlineData("", "2", "default", "2")]
        [InlineData(null, "  ", "default", "--empty--")] // whitespace is a valid value
        // no keys set
        [InlineData(null, null, "default", "default")]
        public void GetCommaSeparatedList_2keys_respectsOrdering(string primaryValue, string secondaryValue, string defaultValue, string expected)
        {
            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "primary", primaryValue },
                    { "secondary", secondaryValue }
                }).Build();

            var actual = conf.GetCommaSeparatedList("primary", "secondary", new List<string> { defaultValue });

            if (expected.Equals("--empty--"))
            {
                Assert.Empty(actual);
            }
            else
            {
                Assert.Single(actual);
                Assert.Equal(expected, actual[0]);
            }
        }

        [Theory]
        [InlineData("1", "2", "3", "default", "1")]
        [InlineData("   ", "2", "3", "default", "--empty--")] // whitespace is a valid value
        // primary not set
        [InlineData(null, "2", "3", "default", "2")]
        [InlineData("", "2", "3", "default", "2")]
        [InlineData(null, "  ", "3", "default", "--empty--")] // whitespace is a valid value
        // primary and secondary not set
        [InlineData(null, null, "3", "default", "3")]
        [InlineData(null, "", "3", "default", "3")]
        [InlineData(null, null, "   ", "default", "--empty--")] // whitespace is a valid value
        // no keys set
        [InlineData(null, null, null, "default", "default")]
        [InlineData(null, null, "", "default", "default")]
        public void GetCommaSeparatedList_3keys_respectsOrdering(string primaryValue, string secondaryValue, string tertiaryValue, string defaultValue, string expected)
        {
            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    { "primary", primaryValue },
                    { "secondary", secondaryValue },
                    { "tertiary", tertiaryValue }
                }).Build();

            var actual = conf.GetCommaSeparatedList("primary", "secondary", "tertiary", new List<string> { defaultValue });

            if (expected.Equals("--empty--"))
            {
                Assert.Empty(actual);
            }
            else
            {
                Assert.Single(actual);
                Assert.Equal(expected, actual[0]);
            }
        }
        #endregion

        #region Victims
        private enum MyTestConfigThing
        {
            Zero = 0,
            One = 1
        }
        #endregion
    }
}