using Minibank.Core.Services;
using System;

namespace Minibank.Data.Services
{
    public class CurrencyRate : ICurrencyRate
    {
        public double GetCurrencyRate(string currencyCode)
        {
            var min = 1;
            var max = 100;
            var rnd = new Random();
            return rnd.NextDouble() * (max - min);
        }
    }
}
