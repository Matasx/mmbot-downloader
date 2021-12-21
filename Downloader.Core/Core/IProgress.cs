namespace Downloader.Core.Core
{
    public interface IProgress
    {
        public void Report(string name, int current, int total);
    }
}