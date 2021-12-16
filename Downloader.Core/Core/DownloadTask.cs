namespace Downloader.Core.Core
{
    public struct DownloadTask
    {
        public string Exchange { get; private set; }
        public string Symbol { get; private set; }
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }        

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
