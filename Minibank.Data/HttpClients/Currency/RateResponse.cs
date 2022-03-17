using System;
using System.Collections.Generic;

namespace Minibank.Data.HttpClients.Models
{
    public class RateResponse
    {
        public DateTime Date { get; set; }
        public Dictionary<string, CurrencyInfo> Valute { get; set; }
    }
}
