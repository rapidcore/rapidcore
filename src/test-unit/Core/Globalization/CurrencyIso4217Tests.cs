using RapidCore.Globalization;
using Xunit;

namespace UnitTests.Core.Globalization
{
    public class CurrencyIso4217Tests
    {
        private readonly Iso4217Currencies currencies;

        public CurrencyIso4217Tests()
        {
            currencies = new Iso4217Currencies();
        }

        [Theory]
        [InlineData("dkk", null, false)] // null is not a currency
        [InlineData("fjd", "fjd", true)] // same alpha
        [InlineData("fjd", "FJD", true)] // same alpha, different case
        [InlineData("fjd", "242", true)] // same numeric
        [InlineData("fjd", "dkk", false)] // not the same
        [InlineData("fjd", "alala", false)] // not even a currency
        public void Is(string currencyCode, string isParam, bool expected)
        {
            Assert.Equal(expected, currencies.Get(currencyCode).Is(isParam));
        }
    }
}