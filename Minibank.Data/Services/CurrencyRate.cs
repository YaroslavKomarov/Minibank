using Minibank.Core.Services;
using System;

namespace Minibank.Data.Services
{
    public class CurrencyRate : ICurrencyRate
    {
        private static readonly Random random = new Random();

        public double GetCurrencyRate(string currencyCode)
        {
            var min = 1;
            var max = 100;
            return random.NextDouble() * (max - min);
        }
    }
}
