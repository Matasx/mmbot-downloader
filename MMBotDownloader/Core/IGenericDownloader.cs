namespace MMBotDownloader.Core
{
    internal interface IGenericDownloader
    {
        string Name { get; }
        string SymbolExample { get; }
        int DegreeOfParallelism { get; }
        void DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask);
    }
}
