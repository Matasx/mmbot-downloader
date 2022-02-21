using Downloader.Core.Core;
using Downloader.Core.Exchange.Common;
using Downloader.Core.Utils;
using System.Text.Json;

namespace Downloader.Core.Exchange.Bybit
{
    public class BybitUsdtPerpDownloader : IDownloader<SecChunk>
    {
        private readonly HttpClient _client;

        private const string ApiBase = "https://api.bybit.com/";

        public string Name => "BYBIT-USDT-PERP";
        public string SymbolExample => "BTCUSDT";

        public int DegreeOfParallelism => 5;

        public BybitUsdtPerpDownloader(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<SecChunk> PrepareChunks(DownloadTask downloadTask) => downloadTask.ToSecChunks(200);

        public async Task<IEnumerable<Kline>> DownloadLinesAsync(SecChunk chunk)
        {
            var url = $"{ApiBase}public/linear/kline?interval=1&symbol={chunk.Symbol}&from={chunk.StartTimeSec}&limit=200";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<BybitResponse<BybitUsdtPerpKline>>(dataString);

            return data.Result.Select(x => new Kline(UnixEpoch.GetDateTimeSec(x.OpenTime), x.Close.ToString().Replace(',', '.')));
        }

        public string DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            return orchestrator.Download(this, downloadTask);
        }

        public async Task<IEnumerable<SymbolInfo>> GetSymbolsAsync()
        {
            const string url = $"{ApiBase}v2/public/symbols";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<BybitResponse<BybitSymbol>>(dataString);

            return data.Result.Select(x => new SymbolInfo(x.Name, x.BaseCurrency, x.QuoteCurrency));
        }
    }
}
