using System.Text.Json.Serialization;

namespace Downloader.Core.Exchange.Kucoin
{
    public class KucoinResponse<T>
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("data")]
        public List<T> Data { get; set; }
    }
}
