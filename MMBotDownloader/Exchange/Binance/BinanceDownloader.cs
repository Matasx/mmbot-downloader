using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using MMBotDownloader.Core;
using MMBotDownloader.Utils;

namespace MMBotDownloader.Exchange.Binance
{
    internal class BinanceDownloader : IDownloader<BinanceChunk>
    {
        private readonly HttpClient _client;

        public string Name => "BINANCE";
        public string SymbolExample => "ADAUSDT";

        public BinanceDownloader(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<BinanceChunk> PrepareChunks(DownloadTask downloadTask)
        {
            var startMs = UnixEpoch.GetEpochMs(downloadTask.Start);
            var endMs = UnixEpoch.GetEpochMs(downloadTask.End);
            var increment = 60000 * 1000L;
            var count = (int)((endMs - startMs) / increment);

            return Enumerable
                .Range(0, count)
                .Select(i => new BinanceChunk(downloadTask.Symbol, startMs + i * increment));
        }

        public async Task<IEnumerable<string>> DownloadLinesAsync(BinanceChunk chunk)
        {
            var url = $"https://api.binance.com/api/v3/klines?symbol={chunk.Symbol}&interval=1m&startTime={chunk.StartTimeMs}&limit=1000";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<IList<IList<object>>>(dataString);
            return data.Select(x => x[4].ToString());
        }

        public void DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            orchestrator.Download(this, downloadTask);
        }
    }
}
