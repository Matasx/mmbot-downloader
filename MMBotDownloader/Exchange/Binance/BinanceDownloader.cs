﻿using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using MMBotDownloader.Core;
using MMBotDownloader.Utils;
using MMBotDownloader.Exchange.Common;

namespace MMBotDownloader.Exchange.Binance
{
    internal class BinanceDownloader : IDownloader<MsChunk>
    {
        private readonly HttpClient _client;

        public string Name => "BINANCE";
        public string SymbolExample => "ADAUSDT";

        public int DegreeOfParallelism => 10;

        public BinanceDownloader(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<MsChunk> PrepareChunks(DownloadTask downloadTask) => downloadTask.ToMsChunks(1000);

        public async Task<IEnumerable<string>> DownloadLinesAsync(MsChunk chunk)
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
