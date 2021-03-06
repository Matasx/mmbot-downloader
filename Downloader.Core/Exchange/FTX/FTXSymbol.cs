using System.Text.Json.Serialization;

namespace Downloader.Core.Exchange.FTX
{
    public class FTXSymbol
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("baseCurrency")]
        public string BaseCurrency { get; set; }

        [JsonPropertyName("quoteCurrency")]
        public string QuoteCurrency { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("underlying")]
        public string Underlying { get; set; }
    }
}