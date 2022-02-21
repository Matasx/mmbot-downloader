using System.Text.Json.Serialization;

namespace Downloader.Core.Exchange.Bybit
{
    public class BybitSymbol
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("base_currency")]
        public string BaseCurrency { get; set; }

        [JsonPropertyName("quote_currency")]
        public string QuoteCurrency { get; set; }
    }
}