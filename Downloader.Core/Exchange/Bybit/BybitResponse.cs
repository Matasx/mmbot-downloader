using System.Text.Json.Serialization;

namespace Downloader.Core.Exchange.Bybit
{
    public class BybitResponse<T>
    {
        [JsonPropertyName("ret_code")]
        public int RetCode { get; set; }

        [JsonPropertyName("ret_msg")]
        public string RetMsg { get; set; }

        [JsonPropertyName("result")]
        public List<T> Result { get; set; }
    }
}
