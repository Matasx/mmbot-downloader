using Downloader.Core.Core;
using Downloader.Core.Exchange.Common;
using Downloader.Core.Utils;
using Newtonsoft.Json;

namespace Downloader.Core.Exchange.Binance
{
    public class BinanceDownloader : IDownloader<MsChunk>
    {
        private readonly HttpClient _client;

        public string Name => "BINANCE";
        public string SymbolExample => "ADAUSDT";

        public int DegreeOfParallelism => 10;

        public BinanceDownloader(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<MsChunk> PrepareChunks(DownloadTask downloadTask) => downloadTask.ToMsChunks(1000);

        public async Task<IEnumerable<Kline>> DownloadLinesAsync(MsChunk chunk)
        {
            var url = $"https://api.binance.com/api/v3/klines?symbol={chunk.Symbol}&interval=1m&startTime={chunk.StartTimeMs}&limit=1000";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<IList<IList<object>>>(dataString);
            return data.Select(x => new Kline(UnixEpoch.GetDateTimeMs((long)x[0]), x[4].ToString()));
        }

        public string DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            return orchestrator.Download(this, downloadTask);
        }
    }
}
