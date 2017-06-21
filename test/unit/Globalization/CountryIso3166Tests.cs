using RapidCore.Globalization;
using Xunit;

namespace RapidCore.UnitTests.Globalization
{
    public class CountryIso3166Tests
    {
        private readonly Iso3166Countries countries;

        public CountryIso3166Tests()
        {
            countries = new Iso3166Countries();
        }

        [Theory]
        [InlineData("dk", "dk", true)] // same alpha-2
        [InlineData("dk", "dnk", true)] // same alpha-3
        [InlineData("dk", "208", true)] // same numeric
        [InlineData("dk", "se", false)] // not the same
        [InlineData("dk", "alala", false)] // not even a country
        public void Is(string countryCode, string isParam, bool expected)
        {
            Assert.Equal(expected, countries.Get(countryCode).Is(isParam));
        }
    }
}