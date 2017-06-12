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
        public void Get_numeric()
        {
            var actual = countries.Get(208);
            
            Assert.Equal("DK", actual.CodeAlpha2);
            Assert.Equal("DNK", actual.CodeAlpha3);
            Assert.Equal(208, actual.CodeNumeric);
            Assert.Equal("Denmark", actual.NameEnglish);
        }
    }
}