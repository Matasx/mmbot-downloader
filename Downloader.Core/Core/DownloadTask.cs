namespace Downloader.Core.Core
{
    public struct DownloadTask
    {
        public string Exchange { get; }
        public string Symbol { get; }
        public DateTime Start { get; }
        public DateTime End { get; }

        public DownloadTask(string exchange, string symbol, DateTimeRange range) : this (exchange, symbol, range.Start, range.End)
        { }

        public DownloadTask(string exchange, string symbol, DateTime start, DateTime end)
        {
            Exchange = exchange;
            Symbol = symbol;
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return $"{Exchange}/{Symbol} ({Start.ToShortDateString()}-{End.ToShortDateString()})";
        }
    }
}
