namespace MMBotDownloader.Core
{
    internal interface IGenericDownloader
    {
        string Name { get; }
        string SymbolExample { get; }
        void DownloadWith(DownloadOrchestrator orchestrator, DownloadTask downloadTask);
    }
}
