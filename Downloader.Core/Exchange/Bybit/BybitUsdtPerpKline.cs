using System.Text.Json.Serialization;

namespace Downloader.Core.Exchange.Bybit
{
    public class BybitUsdtPerpKline
    {
        [JsonPropertyName("open_time")]
        public long OpenTime { get; set; }

        [JsonPropertyName("close")]
        public double Close { get; set; }
    }
}