using System.Threading.Tasks;
using FluentValidation;
using System.Threading;
using System;

namespace Minibank.Core.Domain.Currency.Services
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private static readonly ValidCurrencies mainCurrency = ValidCurrencies.RUB;

        private readonly IValidator<ConvertCurrencyDto> convertCurrencyValidator;

        private readonly ICurrencyRateService currencyRateService;

        public CurrencyConverterService(
            ICurrencyRateService currencyRateService, 
            IValidator<ConvertCurrencyDto> convertCurrencyValidator)
        {
            this.currencyRateService = currencyRateService;
            this.convertCurrencyValidator = convertCurrencyValidator;
        }
  
        public async Task<decimal> ConvertAsync(
            decimal? amount,
            string fromCurrency,
            string toCurrency, 
            CancellationToken cancellationToken)
        {
            convertCurrencyValidator.ValidateAndThrow(new ConvertCurrencyDto
            {
                Amount = amount,
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency
            });

            fromCurrency = fromCurrency.ToUpperInvariant();
            toCurrency = toCurrency.ToUpperInvariant();

            return mainCurrency switch
            {
                ValidCurrencies.RUB when fromCurrency == toCurrency 
                    => Math.Round(amount.Value, 2),

                ValidCurrencies.RUB when fromCurrency == mainCurrency.ToString() && toCurrency != mainCurrency.ToString() 
                    => await ConvertToCurrency(amount.Value, toCurrency, cancellationToken),

                ValidCurrencies.RUB when fromCurrency != mainCurrency.ToString() && toCurrency != mainCurrency.ToString()
                    => await ConvertBothCurrencies(amount.Value, fromCurrency, toCurrency, cancellationToken),

                _ => await ConvertFromCurrency(amount.Value, fromCurrency, cancellationToken)
            };
        }

        private async Task<decimal> ConvertToCurrency(
            decimal amount,
            string toCurrency,
            CancellationToken cancellationToken)
        {
            var rate = await currencyRateService.GetCurrencyRateAsync(toCurrency, cancellationToken);

            return Math.Round(amount / rate, 2);
        }

        private async Task<decimal> ConvertFromCurrency(
            decimal amount,
            string fromCurrency,
            CancellationToken cancellationToken)
        {
            var rate = await currencyRateService.GetCurrencyRateAsync(fromCurrency, cancellationToken);

            return Math.Round(amount * rate, 2);
        }

        private async Task<decimal> ConvertBothCurrencies(
            decimal amount,
            string fromCurrency,
            string toCurrency,
            CancellationToken cancellationToken)
        {
            var fromRate = await currencyRateService.GetCurrencyRateAsync(fromCurrency, cancellationToken);
            var toRate = await currencyRateService.GetCurrencyRateAsync(toCurrency, cancellationToken);

            return Math.Round((amount * fromRate) / toRate, 2);
        }
    }
}
