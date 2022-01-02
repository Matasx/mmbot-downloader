using System.Text.Json.Serialization;

namespace Downloader.Core.Exchange.FTX
{
    public class FTXSymbolResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("result")]
        public List<FTXSymbol> Result { get; set; }
    }
}