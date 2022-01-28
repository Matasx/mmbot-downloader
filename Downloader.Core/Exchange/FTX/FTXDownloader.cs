using System.Globalization;
using System.Text.Json;
using Downloader.Core.Core;
using Downloader.Core.Exchange.Common;

namespace Downloader.Core.Exchange.FTX
{
    public class FTXDownloader : IDownloader<SecChunk>
    {
        private readonly HttpClient _client;

        private const string ApiBase = "https://ftx.com/api/";

        public string Name => "FTX";
        public string SymbolExample => "ADA-PERP";

        public int DegreeOfParallelism => 10;

        public FTXDownloader(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<SecChunk> PrepareChunks(DownloadTask downloadTask) => downloadTask.ToSecChunks(1500);

        public async Task<IEnumerable<Kline>> DownloadLinesAsync(SecChunk chunk)
        {
            var url = $"{ApiBase}markets/{chunk.Symbol}/candles?resolution=60&start_time={chunk.StartTimeSec}&end_time={chunk.EndTimeSec}";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<FTXResponse>(dataString);
            return data.result.Select(x => new Kline(DateTime.Parse(x.startTime, null, DateTimeStyles.AssumeUniversal).ToUniversalTime(), x.close.ToString("G").Replace(',', '.')));
        }

        public string DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            return orchestrator.Download(this, downloadTask);
        }

        public async Task<IEnumerable<SymbolInfo>> GetSymbolsAsync()
        {
            const string url = $"{ApiBase}markets";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<FTXSymbolResponse>(dataString);
            return data.Result.Select(x => new SymbolInfo(x.Name, x.BaseCurrency, x.QuoteCurrency));
        }
    }
}
