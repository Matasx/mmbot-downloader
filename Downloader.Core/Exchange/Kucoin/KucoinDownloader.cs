using Downloader.Core.Core;
using Downloader.Core.Exchange.Common;
using Downloader.Core.Utils;
using Newtonsoft.Json;

namespace Downloader.Core.Exchange.Kucoin
{
    public class KucoinDownloader : IDownloader<SecChunk>
    {
        private readonly HttpClient _client;

        public string Name => "KUCOIN";
        public string SymbolExample => "BTC-USDT";

        public int DegreeOfParallelism => 3;

        public KucoinDownloader(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<SecChunk> PrepareChunks(DownloadTask downloadTask) => downloadTask.ToSecChunks(1500);

        public async Task<IEnumerable<Kline>> DownloadLinesAsync(SecChunk chunk)
        {
            var url = $"https://api.kucoin.com/api/v1/market/candles?type=1min&symbol={chunk.Symbol}&startAt={chunk.StartTimeSec}&endAt={chunk.EndTimeSec}";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<KucoinResponse>(dataString);

            if (data.Code == "400100") throw new PairNotAvailableException(data.Msg);

            return data.Data.Select(x => new Kline(UnixEpoch.GetDateTimeSec(long.Parse(x[0])), x[2].ToString()));
        }

        public string DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            return orchestrator.Download(this, downloadTask);
        }
    }
}
