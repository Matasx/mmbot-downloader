namespace Downloader.Core.Exchange
{
    internal class PairNotAvailableException : Exception
    {
        public PairNotAvailableException(string message) : base(message)
        {
            
        }
    }
}