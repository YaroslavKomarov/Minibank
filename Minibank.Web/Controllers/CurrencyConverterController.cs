using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Services;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyConverterController : Controller
    {
        private readonly ICurrencyConverter converter;

        public CurrencyConverterController(ICurrencyConverter converter)
        {
            this.converter = converter;
        }

        [HttpGet]
        public double GetConvertCurrency(int amount, string currencyCode)
        {
            return converter.ConvertRubles(amount, currencyCode);
        }
    }
}
