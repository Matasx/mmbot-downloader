using MMBotDownloader.Utils;

namespace MMBotDownloader.Configuration
{
    internal class ConfigProvider : IConfigProvider
    {
        private readonly UserInterface _ui;

        public ConfigProvider(UserInterface ui)
        {
            _ui = ui;
        }

        public Configuration GetConfig()
        {
            var selection = _ui.Prompt("Load settings from Config.json? (y/N): ").Trim().ToLower();
            IConfigProvider provider = selection == "a" || selection == "y" ? new FileConfigProvider() : new UserPromptConfigProvider(_ui);
            return provider.GetConfig();
        }
    }
}
