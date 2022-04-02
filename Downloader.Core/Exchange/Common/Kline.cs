namespace Downloader.Core.Exchange.Common
{
    public class Kline
    {
        public DateTime Time { get; }
        public string Value { get; }
        public string Volume { get; }

        public Kline(DateTime time, string value, string volume)
        {
            Time = time;
            Value = value;
            Volume = volume;
        }

        public override string ToString()
        {
            return $"{Time}: {Value} @{Volume}";
        }

        public string ToCsvLine(bool includeVolume)
        {
            return $"{Value}{(includeVolume ? $",{Volume}" : string.Empty)}";
        }
    }
}