using System.Text.Json.Serialization;

namespace Downloader.Core.Exchange.Bybit
{
    public class BybitKline
    {
        [JsonPropertyName("open_time")]
        public long OpenTime { get; set; }

        [JsonPropertyName("close")]
        public string Close { get; set; }
    }
}