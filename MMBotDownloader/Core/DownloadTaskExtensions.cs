using MMBotDownloader.Exchange.Common;
using MMBotDownloader.Utils;
using System.Collections.Generic;
using System.Linq;

namespace MMBotDownloader.Core
{
    internal static class DownloadTaskExtensions
    {
        public static IEnumerable<SecChunk> ToSecChunks(this DownloadTask downloadTask, int dataCountLimit)
        {
            var startSec = UnixEpoch.GetEpochSec(downloadTask.Start);
            var endSec = UnixEpoch.GetEpochSec(downloadTask.End);
            var increment = dataCountLimit * 60;
            var count = (int)((endSec - startSec) / increment);

            return Enumerable
                .Range(0, count)
                .Select(i => new SecChunk(downloadTask.Symbol, startSec + i * increment, startSec + (i + 1) * increment));
        }
    }
}
