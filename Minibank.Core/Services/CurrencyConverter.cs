using System;

namespace Minibank.Core.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly ICurrencyRate currencyRate;
        private static readonly string mainCurrencyCode = "RUB";

        public CurrencyConverter(ICurrencyRate currencyRate)
        {
            this.currencyRate = currencyRate;
        }
  
        public decimal Convert(decimal? amount, string fromCurrency, string toCurrency)
        {
            ValidateArguments(amount, fromCurrency, toCurrency);

            var validAmount = (decimal)amount;

            if (fromCurrency == mainCurrencyCode && toCurrency == mainCurrencyCode)
            {
                return Math.Round(validAmount, 2);
            }
            else if (fromCurrency == mainCurrencyCode && toCurrency != mainCurrencyCode)
            {
                return ConvertFromMainCurrency(validAmount, toCurrency);
            }
            else if (fromCurrency != mainCurrencyCode && toCurrency != mainCurrencyCode)
            {
                return ConverBothNonMainCurrencies(validAmount, fromCurrency, toCurrency);
            }
            else
            {
                return ConvertToMainCurrency(validAmount, fromCurrency);
            }
        }

        private decimal ConvertFromMainCurrency(decimal amount, string toCurrency)
        {
            var currencyAmount = amount / currencyRate.GetCurrencyRate(toCurrency);
            return Math.Round(currencyAmount, 2);
        }

        private decimal ConverBothNonMainCurrencies(decimal amount, string fromCurrency, string toCurrency)
        {
            var mainCurrencyAmount = amount * currencyRate.GetCurrencyRate(fromCurrency);
            return ConvertFromMainCurrency(mainCurrencyAmount, toCurrency);
        }

        private decimal ConvertToMainCurrency(decimal amount, string fromCurrency)
        {
            var currencyAmount = amount * currencyRate.GetCurrencyRate(fromCurrency);
            return Math.Round(currencyAmount, 2);
        }

        private static void ValidateArguments(decimal? amount, string fromCurrency, string toCurrency)
        {
            if (amount == null || amount < 0
                || string.IsNullOrWhiteSpace(toCurrency)
                || string.IsNullOrWhiteSpace(fromCurrency))
            {
                throw new ValidationException("Сумма недействительна или валютный(ые) код(ы) пуст(ы)");
            }
        }
    }
}
