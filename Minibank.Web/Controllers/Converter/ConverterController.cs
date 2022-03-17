using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Services;

namespace Minibank.Web.Controllers.Converter
{
    [ApiController]
    [Route("api/v1/minibank/[controller]/[action]")]
    public class ConverterController : ControllerBase
    {
        private readonly ICurrencyConverter converter;

        public ConverterController(ICurrencyConverter converter)
        {
            this.converter = converter;
        }

        [HttpGet]
        public decimal Convert(ConverterDto model)
        {
            return converter.Convert(
                model.amount, 
                model.fromCurrency, 
                model.toCurrency);
        }
    }
}
