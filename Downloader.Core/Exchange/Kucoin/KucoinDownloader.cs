using Downloader.Core.Core;
using Downloader.Core.Exchange.Common;
using Downloader.Core.Utils;
using Newtonsoft.Json;

namespace Downloader.Core.Exchange.Kucoin
{
    public class KucoinDownloader : IDownloader<SecChunk>
    {
        private readonly HttpClient _client;

        private const string ApiBase = "https://api.kucoin.com/api/v1/";

        public string Name => "KUCOIN";
        public string SymbolExample => "BTC-USDT";

        public int DegreeOfParallelism => 5;

        public KucoinDownloader(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<SecChunk> PrepareChunks(DownloadTask downloadTask) => downloadTask.ToSecChunks(1500);

        public async Task<IEnumerable<Kline>> DownloadLinesAsync(SecChunk chunk)
        {
            var url = $"{ApiBase}market/candles?type=1min&symbol={chunk.Symbol}&startAt={chunk.StartTimeSec}&endAt={chunk.EndTimeSec}";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<KucoinResponse<List<string>>>(dataString);

            if (data.Code == "400100") throw new PairNotAvailableException(data.Msg);

            return data.Data.Select(x => new Kline(UnixEpoch.GetDateTimeSec(long.Parse(x[0])), x[2].ToString()));
        }

        public string DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            return orchestrator.Download(this, downloadTask);
        }

        public async Task<IEnumerable<SymbolInfo>> GetSymbolsAsync()
        {
            const string url = $"{ApiBase}symbols";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<KucoinResponse<KucoinSymbol>>(dataString);

            return data.Data.Select(x => new SymbolInfo(x.Symbol, x.BaseCurrency, x.QuoteCurrency));
        }
    }
}
