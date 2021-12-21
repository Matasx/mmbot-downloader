using Downloader.Core.Exchange.Binance;
using Downloader.Core.Exchange.Common;

namespace Downloader.Core.Core
{
    public interface IDownloader<T> : IGenericDownloader where T : struct
    {
        IEnumerable<T> PrepareChunks(DownloadTask downloadTask);

        Task<IEnumerable<Kline>> DownloadLinesAsync(T chunk);
    }
}
