using Downloader.Core.Core;
using Downloader.Core.Exchange.Common;
using Downloader.Core.Utils;
using System.Diagnostics;
using System.Text.Json;

namespace Downloader.Core.Exchange.Bitfinex
{
    public class BitfinexDownloader : IDownloader<MsChunk>
    {
        private readonly HttpClient _client;

        private const string ApiBase = "https://api-pub.bitfinex.com/v2/";

        public string Name => "BITFINEX";
        public string SymbolExample => "tBTCUSD";

        // Bitfinex API is heavily speed-limited
        public int DegreeOfParallelism => 1;
        private const int RequestAvgMs = 1000; // equals to 60 rpm

        public BitfinexDownloader(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<MsChunk> PrepareChunks(DownloadTask downloadTask) => downloadTask.ToMsChunks(10000);

        public async Task<IEnumerable<Kline>> DownloadLinesAsync(MsChunk chunk)
        {
            var watch = Stopwatch.StartNew();
            var url = $"{ApiBase}candles/trade:1m:{chunk.Symbol}/hist?limit=10000&start={chunk.StartTimeMs}&end={chunk.EndTimeMs}&sort=1";
            var dataString = await _client.GetStringAsync(url);            
            var data = JsonSerializer.Deserialize<IList<IList<object>>>(dataString);
            var result = data.Select(x => new Kline(UnixEpoch.GetDateTimeMs((long)x[0]), x[2].ToString().Replace(',', '.'), "0")); //TODO: volume
            await Task.Delay(Math.Max(0, (int)(RequestAvgMs - watch.ElapsedMilliseconds)));
            return result;
        }

        public string DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            return orchestrator.Download(this, downloadTask);
        }

        public async Task<IEnumerable<SymbolInfo>> GetSymbolsAsync()
        {
            const string url = $"{ApiBase}conf/pub:list:pair:exchange";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<IList<IList<string>>>(dataString);
            return data.SelectMany(x => x.Select(GetSymbolInfo));
        }

        private static string ToSymbol(string asset, string currency)
        {
            return $"t{asset}{currency}";
        }

        private static SymbolInfo GetSymbolInfo(string symbol)
        {
            var split = symbol.Split(':');
            if (split.Length == 2)
            {
                return new SymbolInfo(ToSymbol(split[0], split[1]), split[0], split[1]);
            }

            var asset = symbol[..3];
            var currency = symbol[3..];
            return new SymbolInfo(ToSymbol(asset, currency), asset, currency);
        }
    }
}
