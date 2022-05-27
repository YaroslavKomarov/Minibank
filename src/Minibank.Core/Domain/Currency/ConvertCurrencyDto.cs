using System;

namespace Minibank.Core.Domain.Currency
{
    public class ConvertCurrencyDto
    {
        public decimal? Amount { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
    }
}
