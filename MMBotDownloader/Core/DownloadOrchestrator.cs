using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.IO;
using System.Diagnostics;
using MMBotDownloader.Utils;

namespace MMBotDownloader.Core
{
    internal class DownloadOrchestrator
    {
        private readonly UserInterface _ui;
        private readonly IGenericDownloader[] _downloaders;

        public DownloadOrchestrator(UserInterface ui, IGenericDownloader[] downloaders)
        {
            _ui = ui;
            _downloaders = downloaders;
        }

        public void PrintExchanges()
        {
            _ui.WriteLine("Available exchanges (name / pair example):");
            foreach (var downloader in _downloaders)
            {
                _ui.WriteSelection($"{downloader.Name}:", downloader.SymbolExample);
            }
            _ui.WriteLine();
        }

        public void Download(DownloadTask downloadTask)
        {
            foreach (var downloader in _downloaders.Where(x => x.Name == downloadTask.Exchange))
            {
                downloader.DownloadWith(this, downloadTask);
            }
        }

        public void Download<T>(IDownloader<T> downloader, DownloadTask downloadTask) where T : struct
        {
            var fileName = $"{downloadTask.Exchange}_{downloadTask.Symbol}_{downloadTask.Start.ToShortDateString()}_{downloadTask.End.ToShortDateString()}.csv";
            foreach (var ch in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(ch.ToString(), string.Empty);
            }

            _ui.WriteSelection("Downloading:", downloadTask.ToString());

            if (File.Exists(fileName))
            {
                _ui.WriteLine($"{fileName} already exists, skipping.");
                return;
            }

            var chunks = downloader.PrepareChunks(downloadTask).ToList();
            var results = new IList<string>[chunks.Count];
            var tasks = chunks.Select<T, Action>((x, i) => () =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Restart();
                results[i] = downloader.DownloadLinesAsync(x).GetAwaiter().GetResult().ToList();
                stopwatch.Stop();
                _ui.WriteSelection($"Downloaded in {stopwatch.ElapsedMilliseconds} ms:", x.ToString());
            });
            var queue = new Queue<Action>(tasks);
            var awaits = new List<Task>();
            using var semaphore = new SemaphoreSlim(downloader.DegreeOfParallelism);

            _ui.WriteSelection("Number of chunks to download:", queue.Count.ToString());
            while (queue.Any())
            {
                semaphore.Wait();
                var action = queue.Dequeue();
                awaits.Add(Task.Run(() =>
                {
                    try
                    {
                        Exception lastException = null;
                        for (var i = 0; i < 3; i++)
                        {
                            try
                            {
                                action();
                                return;
                            }
                            catch (Exception e)
                            {
                                lastException = e;
                            }
                        }
                        _ui.WriteLine($"Exception during download - data will be incomplete:{Environment.NewLine}{lastException}", ConsoleColor.Red);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }
            Task.WaitAll(awaits.ToArray());

            _ui.WriteSelection("Writing:", fileName);

            using var writer = new StreamWriter(fileName, true);
            foreach (var line in results.Where(x => x != null).SelectMany(x => x))
            {
                writer.WriteLine(line);
            }
        }
    }
}
