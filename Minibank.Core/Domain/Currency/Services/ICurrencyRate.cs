using System;

namespace Minibank.Core.Services
{
    public interface ICurrencyRate
    {
        public decimal GetCurrencyRate(string currencyCode);
    }
}

