using System.Globalization;
using Downloader.Core.Core;

namespace Downloader.Core.Configuration
{
    public static class ConfigurationEntryExtensions
    {
        public static DownloadTask ToDownloadTask(this ConfigurationEntry configuration, bool downloadVolume)
        {
            return new DownloadTask(
                "data",
                configuration.Exchange,
                configuration.TradingPair,
                DateTime.Parse(configuration.StartDate, null, DateTimeStyles.AssumeUniversal).ToUniversalTime(),
                DateTime.Parse(configuration.StopDate, null, DateTimeStyles.AssumeUniversal).ToUniversalTime(),
                downloadVolume);
        }
    }
}
