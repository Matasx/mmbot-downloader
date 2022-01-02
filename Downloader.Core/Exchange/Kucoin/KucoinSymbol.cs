using System.Text.Json.Serialization;

namespace Downloader.Core.Exchange.Kucoin
{
    public class KucoinSymbol
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("baseCurrency")]
        public string BaseCurrency { get; set; }

        [JsonPropertyName("quoteCurrency")]
        public string QuoteCurrency { get; set; }

        [JsonPropertyName("feeCurrency")]
        public string FeeCurrency { get; set; }

        [JsonPropertyName("market")]
        public string Market { get; set; }

        [JsonPropertyName("baseMinSize")]
        public string BaseMinSize { get; set; }

        [JsonPropertyName("quoteMinSize")]
        public string QuoteMinSize { get; set; }

        [JsonPropertyName("baseMaxSize")]
        public string BaseMaxSize { get; set; }

        [JsonPropertyName("quoteMaxSize")]
        public string QuoteMaxSize { get; set; }

        [JsonPropertyName("baseIncrement")]
        public string BaseIncrement { get; set; }

        [JsonPropertyName("quoteIncrement")]
        public string QuoteIncrement { get; set; }

        [JsonPropertyName("priceIncrement")]
        public string PriceIncrement { get; set; }

        [JsonPropertyName("priceLimitRate")]
        public string PriceLimitRate { get; set; }

        [JsonPropertyName("isMarginEnabled")]
        public bool IsMarginEnabled { get; set; }

        [JsonPropertyName("enableTrading")]
        public bool EnableTrading { get; set; }
    }
}