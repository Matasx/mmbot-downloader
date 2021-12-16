using Downloader.Core.Utils;

namespace Downloader.Core.Exchange.Common
{
    public struct MsChunk
    {
        public string Symbol { get; private set; }
        public long StartTimeMs { get; private set; }
        public long EndTimeMs { get; private set; }
        private readonly DateTime _startTime;

        public MsChunk(string symbol, long startTimeMs, long endTimeMs)
        {
            Symbol = symbol;
            StartTimeMs = startTimeMs;
            EndTimeMs = endTimeMs;
            _startTime = UnixEpoch.GetDateTimeMs(startTimeMs);
        }

        public override string ToString()
        {
            return _startTime.ToString();
        }
    }
}
