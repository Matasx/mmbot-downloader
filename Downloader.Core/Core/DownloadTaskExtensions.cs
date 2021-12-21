using Downloader.Core.Exchange.Common;
using Downloader.Core.Utils;

namespace Downloader.Core.Core
{
    public static class DownloadTaskExtensions
    {
        internal static IEnumerable<SecChunk> ToSecChunks(this DownloadTask downloadTask, int dataCountLimit)
        {
            var startSec = UnixEpoch.GetEpochSec(downloadTask.Start);
            var endSec = UnixEpoch.GetEpochSec(downloadTask.End);
            var increment = dataCountLimit * 60L;
            var count = (int)((endSec - startSec) / increment);

            return Enumerable
                .Range(0, count)
                .Select(i => new SecChunk(downloadTask.Symbol, startSec + i * increment, startSec + ((i + 1) * increment)));
        }

        internal static IEnumerable<MsChunk> ToMsChunks(this DownloadTask downloadTask, int dataCountLimit)
        {
            var startSec = UnixEpoch.GetEpochMs(downloadTask.Start);
            var endSec = UnixEpoch.GetEpochMs(downloadTask.End);
            var increment = dataCountLimit * 60000L;
            var count = (int)((endSec - startSec) / increment);

            return Enumerable
                .Range(0, count)
                .Select(i => new MsChunk(downloadTask.Symbol, startSec + i * increment, startSec + ((i + 1) * increment)));
        }

        public static string ToFileName(this DownloadTask downloadTask)
        {
            var fileName = $"{downloadTask.Exchange}_{downloadTask.Symbol}_{downloadTask.Start.ToShortDateString()}_{downloadTask.End.ToShortDateString()}.csv";
            fileName = Path.GetInvalidFileNameChars().Aggregate(fileName, (current, ch) => current.Replace(ch.ToString(), string.Empty));

            if (string.IsNullOrEmpty(downloadTask.Folder)) return fileName;

            var directory = new DirectoryInfo(downloadTask.Folder);
            if (!directory.Exists) directory.Create();
            return Path.Combine(directory.FullName, fileName);

        }
    }
}
