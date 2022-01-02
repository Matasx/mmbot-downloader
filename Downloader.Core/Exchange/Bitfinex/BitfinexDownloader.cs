using Downloader.Core.Core;
using Downloader.Core.Exchange.Common;
using Downloader.Core.Utils;
using Newtonsoft.Json;

namespace Downloader.Core.Exchange.Bitfinex
{
    public class BitfinexDownloader : IDownloader<MsChunk>
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

        public async Task<IEnumerable<Kline>> DownloadLinesAsync(MsChunk chunk)
        {
            var url = $"https://api-pub.bitfinex.com/v2/candles/trade:1m:{chunk.Symbol}/hist?limit=10000&start={chunk.StartTimeMs}&end={chunk.EndTimeMs}&sort=1";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<IList<IList<object>>>(dataString);
            return data.Select(x => new Kline(UnixEpoch.GetDateTimeMs((long)x[0]), x[2].ToString().Replace(',', '.')));
        }

        public string DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            return orchestrator.Download(this, downloadTask);
        }
    }
}
