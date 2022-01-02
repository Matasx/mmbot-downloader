namespace Downloader.Core.Core
{
    public class SymbolInfo
    {
        public string Symbol { get; }
        public string Asset { get; }
        public string Currency { get; }

        public SymbolInfo(string symbol, string asset, string currency)
        {
            Symbol = symbol;
            Asset = asset;
            Currency = currency;
        }

        public override string ToString()
        {
            return $"{Symbol} ({Asset}-{Currency})";
        }
    }
}