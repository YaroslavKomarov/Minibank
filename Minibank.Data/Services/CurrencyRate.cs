using Minibank.Core.Services;
using System;

namespace Minibank.Data.Services
{
    public class CurrencyRate : ICurrencyRate
    {
        private static readonly Random random = new Random();

        public decimal GetCurrencyRate(string currencyCode)
        {
            var min = 1;
            var max = 100;
            var currencyRate = (decimal)random.NextDouble() * (max - min);
            return CurrencyConverter.RoundValueToHundredths(currencyRate);
        }
    }
}
