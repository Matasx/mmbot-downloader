using Downloader.Core.Utils;

namespace Downloader.Core.Exchange.Common
{
    public struct SecChunk
    {
        public string Symbol { get; private set; }
        public long StartTimeSec { get; private set; }
        public long EndTimeSec { get; private set; }
        private readonly DateTime _startTime;

        public SecChunk(string symbol, long startTimeSec, long endTimeSec)
        {
            Symbol = symbol;
            StartTimeSec = startTimeSec;
            EndTimeSec = endTimeSec;
            _startTime = UnixEpoch.GetDateTimeSec(startTimeSec);
        }

        public override string ToString()
        {
            return _startTime.ToString();
        }
    }
}
