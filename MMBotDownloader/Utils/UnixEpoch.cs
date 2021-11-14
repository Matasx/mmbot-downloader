using System;

namespace MMBotDownloader.Utils
{
    internal static class UnixEpoch
    {
        public static DateTime GetDateTimeSec(long epoch) => DateTime.UnixEpoch.AddSeconds(epoch);
        public static DateTime GetDateTimeMs(long epoch) => DateTime.UnixEpoch.AddMilliseconds(epoch);
        public static long GetEpochSec(DateTime dateTime) => (long)(dateTime.ToUniversalTime() - DateTime.UnixEpoch).TotalSeconds;
        public static long GetEpochMs(DateTime dateTime) => (long)(dateTime.ToUniversalTime() - DateTime.UnixEpoch).TotalMilliseconds;
    }
}
