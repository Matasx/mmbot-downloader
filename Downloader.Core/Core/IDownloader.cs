namespace Downloader.Core.Core
{
    public interface IDownloader<T> : IGenericDownloader where T : struct
    {
        IEnumerable<T> PrepareChunks(DownloadTask downloadTask);

        Task<IEnumerable<string>> DownloadLinesAsync(T chunk);
    }
}
