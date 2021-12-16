using System.Text.Json.Serialization;

namespace Downloader.Core.Exchange.Kucoin
{
    public class KucoinResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("data")]
        public List<List<string>> Data { get; set; }
    }
}
