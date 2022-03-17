using System.Text.Json.Serialization;

namespace Minibank.Data.HttpClients.Models
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
