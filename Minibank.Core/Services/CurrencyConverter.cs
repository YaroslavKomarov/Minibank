using System;

namespace Minibank.Core.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly ICurrencyRate currencyRate;

        public CurrencyConverter(ICurrencyRate currencyRate)
        {
            this.currencyRate = currencyRate;
        }
  
        public decimal ConvertRubles(decimal? amount, string currencyCode)
        {
            if (amount == null || amount < 0 || string.IsNullOrWhiteSpace(currencyCode))
            {
                throw new InvalidCurrencyArgumentsException("Сумма недействительна или валютный код пуст");
            }
            else
            {
                var currencyAmount = amount / currencyRate.GetCurrencyRate(currencyCode);
                return Math.Round((decimal)currencyAmount, 2);
            }
        }
    }
}
