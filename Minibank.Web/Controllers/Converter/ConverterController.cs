using Minibank.Core.Domain.Currency.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

namespace Minibank.Web.Controllers.Converter
{
    [ApiController]
    [Route("api/v1/minibank/[controller]/[action]")]
    public class ConverterController : ControllerBase
    {
        private readonly ICurrencyConverterService converter;

        public ConverterController(ICurrencyConverterService converter)
        {
            this.converter = converter;
        }

        [HttpGet]
        public async Task<decimal> Convert(
            decimal? amount,
            string fromCurrency,
            string toCurrency,
            CancellationToken cancellationToken)
        {
            return await converter.ConvertAsync(amount, fromCurrency, toCurrency, cancellationToken);
        }
    }
}
