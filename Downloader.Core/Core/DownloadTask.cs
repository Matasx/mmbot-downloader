namespace Downloader.Core.Core
{
    public struct DownloadTask
    {
        public string Folder { get; }
        public string Exchange { get; }
        public string Symbol { get; }
        public DateTime Start { get; }
        public DateTime End { get; }

        public DownloadTask(string folder, string exchange, string symbol, DateTimeRange range) : this (folder, exchange, symbol, range.Start, range.End)
        { }

        public DownloadTask(string folder, string exchange, string symbol, DateTime start, DateTime end)
        {
            Folder = folder;
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
