using Minibank.Core.Domain.Currency;
using System.Threading.Tasks;
using System.Threading;
using System;
using FluentValidation;

namespace Minibank.Core.Services
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
  
        public async Task<decimal> Convert(
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
                    => await ConvertMainCurrency(amount.Value, toCurrency, cancellationToken),

                ValidCurrencies.RUB when fromCurrency != mainCurrency.ToString() && toCurrency != mainCurrency.ToString()
                    => await ConvertToMainCurrency(amount.Value, fromCurrency, toCurrency, cancellationToken),

                _ => await ConvertToMainCurrency(amount.Value, fromCurrency, cancellationToken)
            };
        }

        private async Task<decimal> ConvertMainCurrency(
            decimal amount,
            string toCurrency,
            CancellationToken cancellationToken)
        {
            var rate = await currencyRateService.GetCurrencyRate(toCurrency, cancellationToken);

            return Math.Round(amount / rate, 2);
        }

        private async Task<decimal> ConvertToMainCurrency(
            decimal amount,
            string fromCurrency,
            CancellationToken cancellationToken)
        {
            var rate = await currencyRateService.GetCurrencyRate(fromCurrency, cancellationToken);

            return Math.Round(amount * rate, 2);
        }

        private async Task<decimal> ConvertToMainCurrency(
            decimal amount,
            string fromCurrency,
            string toCurrency,
            CancellationToken cancellationToken)
        {
            var fromRate = await currencyRateService.GetCurrencyRate(fromCurrency, cancellationToken);
            var toRate = await currencyRateService.GetCurrencyRate(toCurrency, cancellationToken);

            return Math.Round((amount * fromRate) / toRate, 2);
        }
    }
}
