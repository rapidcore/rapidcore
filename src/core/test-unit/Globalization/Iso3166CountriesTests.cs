using RapidCore.Globalization;
using Xunit;

namespace RapidCore.UnitTests.Globalization
{
    public class Iso3166CountriesTests
    {
        private readonly Iso3166Countries countries;

        public Iso3166CountriesTests()
        {
            countries = new Iso3166Countries();
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("TotallyNotACountryCode")]
        [InlineData("_208")] // does not register as "numeric", so ends up with null
        public void Get_returnsNull_ifGiven(string given)
        {
            Assert.Null(countries.Get(given));
        }

        [Fact]
        public void Get_alpha2()
        {
            var actual = countries.Get("dk");
            
            Assert.Equal("DK", actual.CodeAlpha2);
            Assert.Equal("DNK", actual.CodeAlpha3);
            Assert.Equal(208, actual.CodeNumeric);
            Assert.Equal("Denmark", actual.NameEnglish);
        }
        
        [Fact]
        public void Get_alpha3()
        {
            var actual = countries.Get("dNk");
            
            Assert.Equal("DK", actual.CodeAlpha2);
            Assert.Equal("DNK", actual.CodeAlpha3);
            Assert.Equal(208, actual.CodeNumeric);
            Assert.Equal("Denmark", actual.NameEnglish);
        }

        [Fact]
        public void Get_numeric_sentInAsString()
        {
            var actual = countries.Get("208");
            
            Assert.Equal("DK", actual.CodeAlpha2);
            Assert.Equal("DNK", actual.CodeAlpha3);
            Assert.Equal(208, actual.CodeNumeric);
            Assert.Equal("Denmark", actual.NameEnglish);
        }
        
        [Fact]
        public void Get_numeric()
        {
            var actual = countries.Get(208);
            
            Assert.Equal("DK", actual.CodeAlpha2);
            Assert.Equal("DNK", actual.CodeAlpha3);
            Assert.Equal(208, actual.CodeNumeric);
            Assert.Equal("Denmark", actual.NameEnglish);
        }

        [Theory]
        [InlineData("alala", "dk", false)] // a is invalid
        [InlineData("dk", "alala", false)] // b is invalid
        [InlineData("dk", "dk", true)] // use same valid country code, should match
        [InlineData("dk", "dnk", true)] // same country different country codes, should match
        [InlineData("dnk", "208", true)] // same country different country codes, should match
        public void Matches(string theOneToCheck, string theOneItShouldBe, bool expected)
        {
            Assert.Equal(expected, countries.Matches(theOneToCheck, theOneItShouldBe));
        }
        
        [Theory]
        // invalid
        [InlineData("alala", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("   ", false)]
        // valid
        [InlineData("dk", true)]
        [InlineData("dK", true)]
        [InlineData("DK", true)]
        [InlineData("dnk", true)]
        [InlineData("dNK", true)]
        [InlineData("208", true)]
        public void IsValidCountryCode(string countryCodeToCheck, bool expected)
        {
            Assert.Equal(expected, countries.IsValidCountryCode(countryCodeToCheck));
        }
    }
}