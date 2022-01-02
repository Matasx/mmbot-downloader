namespace Downloader.Core.Core
{
    public interface IGenericDownloader
    {
        string Name { get; }
        string SymbolExample { get; }
        int DegreeOfParallelism { get; }
        string DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask);
        Task<IEnumerable<SymbolInfo>> GetSymbolsAsync();
    }
}
