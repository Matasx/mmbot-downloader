using Downloader.Core.Utils;

namespace Downloader.Core.Configuration
{
    internal class UserPromptConfigProvider : IConfigProvider
    {
        private readonly UserInterface _ui;

        public UserPromptConfigProvider(UserInterface ui)
        {
            _ui = ui;
        }

        public Configuration GetConfig()
        {
            return new Configuration
            {
                DownloadVolume = _ui.Prompt("Download volume? (y/N): ").Trim().ToLower() == "y",
                Pairs = new List<ConfigurationEntry>
                {
                    new ConfigurationEntry
                    {
                        Exchange = _ui.Prompt("Exchange:"),
                        TradingPair = _ui.Prompt("Symbol:"),
                        StartDate = _ui.Prompt("Start date (1.1.2021):"),
                        StopDate = _ui.Prompt("End date (1.2.2021):")
                    }
                }
            };
        }
    }
}
