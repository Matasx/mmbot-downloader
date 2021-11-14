using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using MMBotDownloader.Core;
using MMBotDownloader.Utils;

namespace MMBotDownloader.Exchange.FTX
{
    internal class FTXDownloader : IDownloader<FTXChunk>
    {
        private readonly HttpClient _client;

        public string Name => "FTX";
        public string SymbolExample => "ADA-PERP";

        public FTXDownloader(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<FTXChunk> PrepareChunks(DownloadTask downloadTask)
        {
            var startSec = UnixEpoch.GetEpochSec(downloadTask.Start);
            var endSec = UnixEpoch.GetEpochSec(downloadTask.End);
            var increment = 90000;
            var count = (int)((endSec - startSec) / increment);

            return Enumerable
                .Range(0, count)
                .Select(i => new FTXChunk(downloadTask.Symbol, startSec + i * increment, startSec + (i + 1) * increment));
        }

        public async Task<IEnumerable<string>> DownloadLinesAsync(FTXChunk chunk)
        {
            var url = $"https://ftx.com/api/markets/{chunk.Symbol}/candles?resolution=60&start_time={chunk.StartTimeSec}&end_time={chunk.EndTimeSec}";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<FTXResponse>(dataString);
            return data.result.Select(x => x.close.ToString("G").Replace(',', '.'));
        }

        public void DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            orchestrator.Download(this, downloadTask);
        }
    }
}
