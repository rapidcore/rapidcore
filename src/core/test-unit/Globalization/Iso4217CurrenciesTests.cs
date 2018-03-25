using System;
using RapidCore.Globalization;
using Xunit;

namespace RapidCore.UnitTests.Globalization
{
    public class Iso4217CurrenciesTests
    {
        private readonly Iso4217Currencies currencies;

        public Iso4217CurrenciesTests()
        {
            currencies = new Iso4217Currencies();
        }

        [Fact]
        public void HasNoDuplicate_Alphacodes()
        {
            foreach (var currency in currencies.GetAll())
            {
                try
                {
                    currencies.Get(currency.CodeAlpha);
                }
                catch (Exception)
                {
                    throw new Exception($"Duplicate detected for {currency.CodeAlpha}");
                }
            }
        }
        
        [Fact]
        public void HasNoDuplicate_Numeric()
        {
            foreach (var currency in currencies.GetAll())
            {
                try
                {
                    currencies.Get(currency.CodeNumeric);
                }
                catch (Exception)
                {
                    throw new Exception($"Duplicate detected for {currency.CodeNumeric}");
                }
            }
        }

        [Fact]
        public void Get_alpha()
        {
            var actual = currencies.Get("iNr");

            Assert.Equal("Indian Rupee", actual.NameEnglish);
            Assert.Equal(356, actual.CodeNumeric);
            Assert.Equal("INR", actual.CodeAlpha);
            Assert.Equal(2, actual.MinorUnit);
        }
        
        [Fact]
        public void Get_alpha_returnsNull_onMiss()
        {
            Assert.Null(currencies.Get("totally not a currency code"));
        }
        
        [Fact]
        public void Get_stringed_numeric()
        {
            var actual = currencies.Get("600");

            Assert.Equal("Guarani", actual.NameEnglish);
            Assert.Equal(600, actual.CodeNumeric);
            Assert.Equal("PYG", actual.CodeAlpha);
            Assert.Equal(0, actual.MinorUnit);
        }

        [Fact]
        public void Get_numeric()
        {
            var actual = currencies.Get(368);

            Assert.Equal("Iraqi Dinar", actual.NameEnglish);
            Assert.Equal(368, actual.CodeNumeric);
            Assert.Equal("IQD", actual.CodeAlpha);
            Assert.Equal(3, actual.MinorUnit);
        }
        
        [Fact]
        public void Get_numeric_returnsNull_onMiss()
        {
            Assert.Null(currencies.Get(123456789));
        }
        
        [Theory]
        [InlineData("alala", "DKK", false)] // a is invalid
        [InlineData("DKK", "alala", false)] // b is invalid
        [InlineData("DKK", "DKK", true)] // use same valid currency code, should match
        [InlineData("840", "usd", true)] // same currency different currency codes, should match
        [InlineData("DKK", "208", true)] // same currency different currency codes, should match
        public void Matches(string theOneToCheck, string theOneItShouldBe, bool expected)
        {
            Assert.Equal(expected, currencies.Matches(theOneToCheck, theOneItShouldBe));
        }
    }
}
