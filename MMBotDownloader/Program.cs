using System.Net.Http;
using System.Net;
using Downloader.Core.Configuration;
using Downloader.Core.Core;
using Downloader.Core.Exchange.Binance;
using Downloader.Core.Exchange.Bitfinex;
using Downloader.Core.Exchange.FTX;
using Downloader.Core.Exchange.Kucoin;
using Downloader.Core.Utils;
using System.Linq;
using log4net;

namespace MMBotDownloader
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        private static void Main()
        {
            ServicePointManager.DefaultConnectionLimit = 10;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Log.Info("Starting");

            var ui = new UserInterface();
            var client = new HttpClient(new TransientErrorRetryHttpClientHandler());
            var orchestrator = new DownloadOrchestrator(ui, new NullProgress(), new IGenericDownloader[] {
                new BinanceDownloader(client),
                new BitfinexDownloader(client),
                new FTXDownloader(client),
                new KucoinDownloader(client)
            });
            orchestrator.PrintExchanges();

            var configuration = new ConfigProvider(ui).GetConfig();
            configuration.PrintTo(ui);

            ui.Prompt("Continue by pressing ENTER ...");
            Log.Info("Downloading");

            foreach (var task in configuration.Pairs.Select(x => x.ToDownloadTask()))
            {
                orchestrator.Download(task);
            }
        }
    }
}
