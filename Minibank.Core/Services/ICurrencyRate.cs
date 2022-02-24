using System;

namespace Minibank.Core.Services
{
    public interface ICurrencyRate
    {
        public double GetCurrencyRate(string currencyCode);
    }
}

