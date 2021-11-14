using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using MMBotDownloader.Core;
using MMBotDownloader.Exchange.Common;

namespace MMBotDownloader.Exchange.Kucoin
{
    internal class BitfinexDownloader : IDownloader<MsChunk>
    {
        private readonly HttpClient _client;

        public string Name => "BITFINEX";
        public string SymbolExample => "tBTCUSD";

        // Bitfinex API is heavily speed-limited
        public int DegreeOfParallelism => 1;

        public BitfinexDownloader(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<MsChunk> PrepareChunks(DownloadTask downloadTask) => downloadTask.ToMsChunks(10000);

        public async Task<IEnumerable<string>> DownloadLinesAsync(MsChunk chunk)
        {
            var url = $"https://api-pub.bitfinex.com/v2/candles/trade:1m:{chunk.Symbol}/hist?limit=10000&start={chunk.StartTimeMs}&end={chunk.EndTimeMs}&sort=1";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<IList<IList<object>>>(dataString);
            return data.Select(x => x[2].ToString().Replace(',', '.'));
        }

        public void DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            orchestrator.Download(this, downloadTask);
        }
    }
}
