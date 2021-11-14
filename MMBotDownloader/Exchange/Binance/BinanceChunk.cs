using MMBotDownloader.Utils;
using System;

namespace MMBotDownloader.Exchange.Binance
{
    internal struct BinanceChunk
    {
        public string Symbol { get; private set; }
        public long StartTimeMs { get; private set; }
        private readonly DateTime _startTime;

        public BinanceChunk(string symbol, long startTimeMs)
        {
            Symbol = symbol;
            StartTimeMs = startTimeMs;
            _startTime = UnixEpoch.GetDateTimeMs(startTimeMs);
        }

        public override string ToString()
        {
            return _startTime.ToString();
        }
    }
}
