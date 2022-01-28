using Downloader.Core.Core;
using Downloader.Core.Exchange.Common;
using Downloader.Core.Utils;
using System.Text.Json;

namespace Downloader.Core.Exchange.Binance
{
    public class BinanceDownloader : IDownloader<MsChunk>
    {
        private readonly HttpClient _client;

        private const string ApiBase = "https://api.binance.com/api/v3/";

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
            var url = $"{ApiBase}klines?symbol={chunk.Symbol}&interval=1m&startTime={chunk.StartTimeMs}&limit=1000";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<IList<IList<object>>>(dataString);
            return data.Select(x => new Kline(UnixEpoch.GetDateTimeMs((long)x[0]), x[4].ToString()));
        }

        public string DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            return orchestrator.Download(this, downloadTask);
        }

        public async Task<IEnumerable<SymbolInfo>> GetSymbolsAsync()
        {
            const string url = $"{ApiBase}exchangeInfo";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<BinanceSymbolResponse>(dataString);
            return data.Symbols.Select(x => new SymbolInfo(x.Symbol, x.BaseAsset, x.QuoteAsset));
        }
    }
}
