﻿using Newtonsoft.Json;

namespace Downloader.Core.Configuration
{
    internal class FileConfigProvider : IConfigProvider
    {
        private readonly string _path;

        public FileConfigProvider(string path = "Config.json")
        {
            _path = path;
        }

        public Configuration GetConfig()
        {
            var content = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<Configuration>(content);
        }
    }
}