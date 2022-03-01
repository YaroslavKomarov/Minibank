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
  
        public decimal ConvertRubles(int amount, string currencyCode)
        {
            if (amount < 0 || string.IsNullOrWhiteSpace(currencyCode))
            {
                throw new InvalidCurrencyArgumentsException("Сумма недействительна или валютный код пуст");
            }
            else
            {
                var currencyAmount = amount / currencyRate.GetCurrencyRate(currencyCode);
                return RoundValueToHundredths(currencyAmount);
            }
        }

        public static decimal RoundValueToHundredths(decimal value)
        {
            return Math.Round(value, 2);
        }
    }
}
