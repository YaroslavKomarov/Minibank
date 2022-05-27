using Minibank.Core.Domain.Currency.CurrencyValidators;
using Minibank.Core.Domain.Currency.Services;
using Minibank.Core.Domain.Currency;
using System.Threading;
using System.Linq;
using Xunit;
using Moq;

namespace Minibank.Core.Tests
{
    public class CurrencyConverterServiceTests
    {
        private readonly Mock<ICurrencyRateService> mockCurrencyRateService;

        private readonly ICurrencyConverterService currencyConverterService;

        private readonly FluentValidation.IValidator<ConvertCurrencyDto> convertCurrencyValidator;

        public CurrencyConverterServiceTests()
        {
            convertCurrencyValidator = new CurrencyValidator();

            mockCurrencyRateService = new Mock<ICurrencyRateService>();

            currencyConverterService = new CurrencyConverterService(mockCurrencyRateService.Object, convertCurrencyValidator);
        }

        [Fact]
        public async void ConvertAsync_WithNullAmount_ShouldThrowException()
        {
            // ARRANGE
            var fromCurrency = "RUB";
            var toCurrency = "RUB";

            // ACT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await currencyConverterService.ConvertAsync(null, fromCurrency, toCurrency, CancellationToken.None)
            );

            // ASSERT
            Assert.Equal("Передана невалидная сумма", exception.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void ConvertAsync_WithLessZeroAmount_ShouldThrowException()
        {
            // ARRANGE
            var fromCurrency = "RUB";
            var toCurrency = "RUB";
            decimal amount = -1;

            // ACT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await currencyConverterService.ConvertAsync(amount, fromCurrency, toCurrency, CancellationToken.None)
            );

            // ASSERT
            Assert.Equal("Передана невалидная сумма", exception.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void ConvertAsync_WithEmptyCurrencies_ShouldThrowException()
        {
            // ARRANGE
            var fromCurrency = "";
            var toCurrency = "RUB";
            decimal amount = 100;

            var fromCurrency2 = "RUB";
            var toCurrency2 = "";

            // ACT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await currencyConverterService.ConvertAsync(amount, fromCurrency, toCurrency, CancellationToken.None)
            );

            var exception2 = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await currencyConverterService.ConvertAsync(amount, fromCurrency2, toCurrency2, CancellationToken.None)
            );

            // ASSERT
            Assert.Equal("Валютный код источника пуст", exception.Errors.Last().ErrorMessage);

            Assert.Equal("Валютный код назначения пуст", exception2.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void ConvertAsync_WithInvalidCurrency_ShouldThrowException()
        {
            // ARRANGE
            var fromCurrency = "invalid";
            var toCurrency = "RUB";
            decimal amount = 100;


            var fromCurrency2 = "RUB";
            var toCurrency2 = "invalid";

            // ACT
            var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await currencyConverterService.ConvertAsync(amount, fromCurrency, toCurrency, CancellationToken.None)
            );

            var exception2 = await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () =>
                await currencyConverterService.ConvertAsync(amount, fromCurrency2, toCurrency2, CancellationToken.None)
            );

            // ASSERT
            Assert.Equal("Недопустимый валютный код источника", exception.Errors.Last().ErrorMessage);

            Assert.Equal("Недопустимый валютный код назначения", exception2.Errors.Last().ErrorMessage);
        }

        [Fact]
        public async void ConvertAsync_WithLowerCurrency_ShouldBeCorrect()
        {
            // ARRANGE
            var fromCurrency = "rub";
            var toCurrency = "rub";
            decimal amount = 100;

            // ACT
            var actualCurrency = await currencyConverterService.ConvertAsync(amount, fromCurrency, toCurrency, CancellationToken.None);

            // ASSERT
            Assert.Equal(amount, actualCurrency);
        }

        [Fact]
        public async void ConvertAsync_ConvertToCurrency_ReturnSuccessPath()
        {
            // ARRANGE
            decimal expectedRate = 1;

            mockCurrencyRateService
                .Setup(service => service.GetCurrencyRateAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(expectedRate);

            var fromCurrency = "rub";
            var toCurrency = "usd";
            decimal amount = 100;

            // ACT
            var actualCurrency = await currencyConverterService.ConvertAsync(amount, fromCurrency, toCurrency, CancellationToken.None);

            // ASSERT
            Assert.Equal(amount, actualCurrency);
        }

        [Fact]
        public async void ConvertAsync_ConvertFromCurrency_ReturnSuccessPath()
        {
            // ARRANGE
            decimal expectedRate = 1;

            mockCurrencyRateService
                .Setup(service => service.GetCurrencyRateAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(expectedRate);

            var fromCurrency = "eur";
            var toCurrency = "rub";
            decimal amount = 100;

            // ACT
            var actualCurrency = await currencyConverterService.ConvertAsync(amount, fromCurrency, toCurrency, CancellationToken.None);

            // ASSERT
            Assert.Equal(amount, actualCurrency);
        }

        [Fact]
        public async void ConvertAsync_ConvertBothCurrencies_ReturnSuccessPath()
        {
            // ARRANGE
            decimal expectedRate = 1;

            mockCurrencyRateService
                .Setup(service => service.GetCurrencyRateAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(expectedRate);

            var fromCurrency = "eur";
            var toCurrency = "usd";
            decimal amount = 100;

            // ACT
            var actualCurrency = await currencyConverterService.ConvertAsync(amount, fromCurrency, toCurrency, CancellationToken.None);

            // ASSERT
            Assert.Equal(amount, actualCurrency);
        }
    }
}
