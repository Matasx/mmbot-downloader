namespace Downloader.Core.Core;

public class NullProgress : IProgress
{
    public void Report(string name, int current, int total)
    {
        
    }
}