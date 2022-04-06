using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Services;
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
        public decimal Convert(
            decimal? amount,
            string fromCurrency,
            string toCurrency,
            CancellationToken cancellationToken)
        {
            return converter.Convert(amount, fromCurrency, toCurrency, cancellationToken);
        }
    }
}
