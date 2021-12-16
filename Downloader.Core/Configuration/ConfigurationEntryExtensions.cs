using System.Globalization;
using Downloader.Core.Core;

namespace Downloader.Core.Configuration
{
    public static class ConfigurationEntryExtensions
    {
        public static DownloadTask ToDownloadTask(this ConfigurationEntry configuration)
        {
            return new DownloadTask(
                configuration.Exchange,
                configuration.TradingPair,
                DateTime.Parse(configuration.StartDate, null, DateTimeStyles.AssumeUniversal),
                DateTime.Parse(configuration.StopDate, null, DateTimeStyles.AssumeUniversal));
        }
    }
}
