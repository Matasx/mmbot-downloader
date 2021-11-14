using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MMBotDownloader.Exchange.Kucoin
{
    public class KucoinResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("data")]
        public List<List<string>> Data { get; set; }
    }
}
