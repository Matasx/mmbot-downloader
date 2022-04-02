namespace Downloader.Core.Core
{
    public struct DownloadTask
    {
        public string Folder { get; }
        public string Exchange { get; }
        public string Symbol { get; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public bool DownloadVolume { get; }

        public DownloadTask(string folder, string exchange, string symbol, DateTimeRange range, bool downloadVolume = false) : this (folder, exchange, symbol, range.Start, range.End, downloadVolume)
        { }

        public DownloadTask(string folder, string exchange, string symbol, DateTime start, DateTime end, bool downloadVolume = false)
        {
            Folder = folder;
            Exchange = exchange;
            Symbol = symbol;
            Start = start;
            End = end;
            DownloadVolume = downloadVolume;
        }

        public override string ToString()
        {
            return $"{Exchange}/{Symbol} ({Start.ToShortDateString()}-{End.ToShortDateString()}){(DownloadVolume ? " + volume" : string.Empty)}";
        }
    }
}
