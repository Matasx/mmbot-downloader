using MMBotDownloader.Core;
using System;
using System.Globalization;

namespace MMBotDownloader.Configuration
{
    internal static class ConfigurationEntryExtensions
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
