namespace Minibank.Web.Controllers.Converter
{
    public class ConverterDto
    {
        public decimal? amount { get; set; }
        public string fromCurrency { get; set; }
        public string toCurrency { get; set; }
    }
}
