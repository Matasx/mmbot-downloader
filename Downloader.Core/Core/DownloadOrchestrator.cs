using System.Diagnostics;
using Downloader.Core.Utils;

namespace Downloader.Core.Core
{
    public class DownloadOrchestrator
    {
        private readonly UserInterface _ui;
        private readonly IProgress _progress;
        private readonly IGenericDownloader[] _downloaders;
        private readonly object _lock = new();

        public DownloadOrchestrator(UserInterface ui, IProgress progress, IGenericDownloader[] downloaders)
        {
            _ui = ui;
            _progress = progress;
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
            foreach (var downloader in _downloaders.Where(x => string.Compare(x.Name, downloadTask.Exchange, StringComparison.InvariantCultureIgnoreCase) == 0))
            {
                downloader.DownloadWith(this, downloadTask);
            }
        }

        public void Download<T>(IDownloader<T> downloader, DownloadTask downloadTask) where T : struct
        {
            var name = downloadTask.ToString();
            _ui.WriteSelection("Downloading:", name);

            var fileName = downloadTask.ToFileName();
            if (File.Exists(fileName))
            {
                _ui.WriteLine($"{fileName} already exists, skipping.");
                return;
            }
            _progress.Report(name, 0, 1);

            var chunks = downloader.PrepareChunks(downloadTask).ToList();
            var results = new IList<string>[chunks.Count];
            var currentChunk = 0;
            var tasks = chunks.Select<T, Action>((x, i) => () =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Restart();
                results[i] = downloader.DownloadLinesAsync(x).GetAwaiter().GetResult().ToList();
                stopwatch.Stop();
                _ui.WriteSelection($"Downloaded in {stopwatch.ElapsedMilliseconds} ms:", x.ToString());
                lock (_lock)
                {
                    currentChunk++;
                    _progress.Report(name, currentChunk, chunks.Count);
                }
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
