using Downloader.Core.Core;
using Downloader.Core.Exchange.Common;
using Downloader.Core.Utils;
using System.Text.Json;

namespace Downloader.Core.Exchange.Bybit
{
    public class BybitDownloader : IDownloader<SecChunk>
    {
        private readonly HttpClient _client;

        private const string ApiBase = "https://api.bybit.com/v2/public/";

        public string Name => "BYBIT";
        public string SymbolExample => "BTCUSD";

        public int DegreeOfParallelism => 5;

        public BybitDownloader(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<SecChunk> PrepareChunks(DownloadTask downloadTask) => downloadTask.ToSecChunks(200);

        public async Task<IEnumerable<Kline>> DownloadLinesAsync(SecChunk chunk)
        {
            var url = $"{ApiBase}kline/list?interval=1&symbol={chunk.Symbol}&from={chunk.StartTimeSec}&limit=200";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<BybitResponse<BybitKline>>(dataString);

            return data.Result.Select(x => new Kline(UnixEpoch.GetDateTimeSec(x.OpenTime), x.Close));
        }

        public string DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            return orchestrator.Download(this, downloadTask);
        }

        public async Task<IEnumerable<SymbolInfo>> GetSymbolsAsync()
        {
            const string url = $"{ApiBase}symbols";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<BybitResponse<BybitSymbol>>(dataString);

            return data.Result.Select(x => new SymbolInfo(x.Name, x.BaseCurrency, x.QuoteCurrency));
        }
    }
}
