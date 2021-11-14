﻿using MMBotDownloader.Utils;

namespace MMBotDownloader.Configuration
{
    internal static class ConfigurationExtensions
    {
        public static void PrintTo(this Configuration configuration, UserInterface ui)
        {
            ui.WriteLine();
            ui.WriteLine("Selected options:");
            foreach (var option in configuration.Pairs)
            {
                ui.WriteSelection("Exchange:", option.Exchange);
                ui.WriteSelection("Symbol:", option.TradingPair);
                ui.WriteSelection("Start:", option.StartDate);
                ui.WriteSelection("End:", option.StopDate);
                ui.WriteLine();
            }
        }
    }
}
