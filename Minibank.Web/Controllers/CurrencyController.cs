using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Services;

namespace Minibank.Web.Controllers
{
    [ApiController]
    [Route("api/v1/minibank/[controller]/[action]")]
    public class CurrencyController : Controller
    {
        private readonly ICurrencyConverter converter;

        public CurrencyController(ICurrencyConverter converter)
        {
            this.converter = converter;
        }

        [HttpGet]
        public decimal Convert(decimal? amount, string currencyCode)
        {
            return converter.ConvertRubles(amount, currencyCode);
        }
    }
}
