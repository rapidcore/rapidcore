using System;
using System.Collections.Generic;
using System.Text;
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
        public void GetCurrencyByCurrencyName()
        {
            var actual = currencies.GetCurrencyByCurrencyName("malaysian ringgit");

            Assert.Equal("Malaysian Ringgit", actual.CurrencyName);
            Assert.Equal(458, actual.NumericCode);
            Assert.Equal("MYR", actual.AlphabeticCode);
        }

        [Fact]
        public void GetCurrencyByAlphabeticCode()
        {
            var actual = currencies.GetCurrencyByAlphabeticCode("inr");

            Assert.Equal("Indian Rupee", actual.CurrencyName);
            Assert.Equal(356, actual.NumericCode);
            Assert.Equal("INR", actual.AlphabeticCode);
        }

        [Fact]
        public void GetCurrencyByNumericCode()
        {
            var actual = currencies.GetCurrencyByNumericCode(943);

            Assert.Equal("Mozambique Metical", actual.CurrencyName);
            Assert.Equal(943, actual.NumericCode);
            Assert.Equal("MZN", actual.AlphabeticCode);
        }

        [Fact]
        public void GetCurrency_ReturnsNull_WhenFiltersFail()
        {
            var actual = currencies.GetCurrencyByNumericCode(1234567890);
            Assert.Null(actual);


            actual = currencies.GetCurrencyByAlphabeticCode("DOES_NOT_EXIST");
            Assert.Null(actual);


            actual = currencies.GetCurrencyByCurrencyName("DOES_NOT_EXIST");
            Assert.Null(actual);
        }
    }
}
