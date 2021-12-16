﻿using Downloader.Core.Core;
using Downloader.Core.Exchange.Common;
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

        public async Task<IEnumerable<string>> DownloadLinesAsync(SecChunk chunk)
        {
            var url = $"https://api.kucoin.com/api/v1/market/candles?type=1min&symbol={chunk.Symbol}&startAt={chunk.StartTimeSec}&endAt={chunk.EndTimeSec}";
            var dataString = await _client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<KucoinResponse>(dataString);
            return data.Data.Select(x => x[2].ToString());
        }

        public void DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask)
        {
            orchestrator.Download(this, downloadTask);
        }
    }
}