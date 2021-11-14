﻿using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using MMBotDownloader.Core;
using MMBotDownloader.Utils;
using MMBotDownloader.Exchange.Common;

namespace MMBotDownloader.Exchange.FTX
{
    internal class FTXDownloader : IDownloader<SecChunk>
    {
        private readonly HttpClient _client;

        public string Name => "FTX";
        public string SymbolExample => "ADA-PERP";

        public int DegreeOfParallelism => 10;

        public FTXDownloader(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<SecChunk> PrepareChunks(DownloadTask downloadTask) => downloadTask.ToSecChunks(1500);

        public async Task<IEnumerable<string>> DownloadLinesAsync(SecChunk chunk)
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
