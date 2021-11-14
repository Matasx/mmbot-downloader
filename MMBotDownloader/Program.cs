using System.Linq;
using System.Net.Http;
using MMBotDownloader.Configuration;
using MMBotDownloader.Core;
using MMBotDownloader.Exchange.FTX;
using MMBotDownloader.Utils;
using MMBotDownloader.Exchange.Binance;
using MMBotDownloader.Exchange.Kucoin;
using System.Net;

namespace MMBotDownloader
{
    internal class Program
    {
        private static void Main()
        {
            ServicePointManager.DefaultConnectionLimit = 10;

            var ui = new UserInterface();
            var client = new HttpClient(new TransientErrorRetryHttpClientHandler());
            var orchestrator = new DownloadOrchestrator(ui, new IGenericDownloader[] {
                new BinanceDownloader(client),
                new FTXDownloader(client),
                new KucoinDownloader(client)
            });
            orchestrator.PrintExchanges();

            var configuration = new ConfigProvider(ui).GetConfig();
            configuration.PrintTo(ui);

            ui.Prompt("Continue by pressing ENTER ...");

            foreach (var task in configuration.Pairs.Select(x => x.ToDownloadTask()))
            {
                orchestrator.Download(task);
            }
        }
    }
}
