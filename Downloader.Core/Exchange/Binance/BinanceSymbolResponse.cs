using System.Text.Json.Serialization;

namespace Downloader.Core.Exchange.Binance
{
    public class BinanceSymbolResponse
    {
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("serverTime")]
        public long ServerTime { get; set; }

        [JsonPropertyName("symbols")]
        public List<BinanceSymbol> Symbols { get; set; }
    }
}