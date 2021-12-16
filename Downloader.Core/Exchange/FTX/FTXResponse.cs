namespace Downloader.Core.Exchange.FTX
{
    internal class FTXResponse
    {
        public bool success { get; set; }
        public List<FTXResult> result { get; set; }
    }
}
