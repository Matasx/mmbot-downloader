namespace Downloader.Core.Exchange.Common
{
    public class Kline
    {
        public DateTime Time { get; }
        public string Value { get; }

        public Kline(DateTime time, string? value)
        {
            Time = time;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Time}: {Value}";
        }
    }
}