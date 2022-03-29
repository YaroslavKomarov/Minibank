using System.Text.Json.Serialization;

namespace Minibank.Data.Currency
{
    public class CurrencyInfo
    {
        [JsonPropertyName("ID")]
        public string Id { get; set; }
        [JsonPropertyName("NumCode")]
        public string NumericCode { get; set; }
        public decimal Value { get; set; }
    }
}
