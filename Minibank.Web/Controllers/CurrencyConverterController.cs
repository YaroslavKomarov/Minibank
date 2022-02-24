using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Services;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyConverterController : Controller
    {
        public CurrencyConverterController(ICurrencyConverter converter)
        {
            _converter = converter;
        }

        [HttpGet]
        public double GetConvertCurrency(int amount, string currencyCode)
        {
            return _converter.ConvertRubles(amount, currencyCode);
        }

        private readonly ICurrencyConverter _converter;
    }
}
