using System;
using System.Collections.Generic;

namespace Minibank.Data.Currency
{
    public class CurrencyRateResponse
    {
        public DateTime Date { get; set; }
        public Dictionary<string, CurrencyInfo> Valute { get; set; }
    }
}
