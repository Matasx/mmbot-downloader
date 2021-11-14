using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMBotDownloader.Core
{
    internal interface IDownloader<T> : IGenericDownloader where T : struct
    {
        IEnumerable<T> PrepareChunks(DownloadTask downloadTask);

        Task<IEnumerable<string>> DownloadLinesAsync(T chunk);
    }
}
