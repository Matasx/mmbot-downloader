using System.Diagnostics;
using Downloader.Core.Exchange;
using Downloader.Core.Exchange.Common;
using Downloader.Core.Utils;
using log4net;

namespace Downloader.Core.Core
{
    public class DownloadOrchestrator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DownloadOrchestrator));

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

        public IGenericDownloader GetDownloader(string exchange)
        {
            return _downloaders
                .FirstOrDefault(x => string.Compare(x.Name, exchange, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public string Download(DownloadTask downloadTask)
        {
            return GetDownloader(downloadTask.Exchange)?.DownloadWith(this, downloadTask);
        }

        public string Download<T>(IDownloader<T> downloader, DownloadTask downloadTask) where T : struct
        {
            var name = downloadTask.ToString();
            Log.Info($"Downloading: {name}");
            _ui.WriteSelection("Downloading:", name);

            var fileName = downloadTask.ToFileName();
            if (File.Exists(fileName))
            {
                Log.Info($"{fileName} already exists, skipping.");
                _ui.WriteLine($"{fileName} already exists, skipping.");
                return fileName;
            }
            _progress.Report(name, 0, 1, false);

            var chunks = downloader.PrepareChunks(downloadTask).ToList();
            var results = new IList<Kline>[chunks.Count];
            var tasks = chunks.Select<T, Action>((x, i) => () =>
            {
                var stopwatch = Stopwatch.StartNew();
                results[i] = downloader.DownloadLinesAsync(x).GetAwaiter().GetResult().ToList();
                stopwatch.Stop();
                Log.Debug($"Downloaded in {stopwatch.ElapsedMilliseconds} ms: {x}");
                _ui.WriteSelection($"Downloaded in {stopwatch.ElapsedMilliseconds} ms:", x.ToString());
            });
            var queue = new Queue<Action>(tasks);
            var awaits = new List<Task>();
            using var semaphore = new SemaphoreSlim(downloader.DegreeOfParallelism);

            Log.Info($"Number of chunks to download: {queue.Count}");
            _ui.WriteSelection("Number of chunks to download:", queue.Count.ToString());

            var currentChunk = 0;
            var stop = false;
            while (!stop && queue.Any())
            {
                semaphore.Wait();
                var action = queue.Dequeue();
                awaits.Add(Task.Run(() =>
                {
                    var error = true;
                    try
                    {
                        Exception lastException = null;
                        for (var i = 0; i < 3; i++)
                        {
                            try
                            {
                                action();
                                error = false;
                                return;
                            }
                            catch (PairNotAvailableException e)
                            {
                                stop = true;
                                Log.Warn(e.Message, e);
                                _ui.WriteLine(e.Message, ConsoleColor.Yellow);
                                return;
                            }
                            catch (Exception e)
                            {
                                Log.Error("Exception during download.", e);
                                lastException = e;
                            }
                        }
                        _ui.WriteLine($"Exception during download - data will be incomplete:{Environment.NewLine}{lastException}", ConsoleColor.Red);
                    }
                    finally
                    {
                        semaphore.Release();
                        lock (_lock)
                        {
                            currentChunk++;
                            _progress.Report(name, currentChunk, chunks.Count, error);
                        }
                    }
                }));
            }
            Task.WaitAll(awaits.ToArray());

            Log.Info($"Writing: {fileName}");
            _ui.WriteSelection("Writing:", fileName);

            using var writer = new StreamWriter(fileName, true);
            Kline previous = null;
            var klines = results
                .Where(x => x != null)
                .SelectMany(x => x)
                .OrderBy(x => x.Time);
            foreach (var kline in klines)
            {
                if (previous != null)
                {
                    var gap = (int)(kline.Time - previous.Time).TotalMinutes - 1;
                    previous = new Kline(previous.Time, previous.Value, "0");
                    for (var i = 0; i < gap; i++)
                    {
                        writer.WriteLine(previous.ToCsvLine(downloadTask.DownloadVolume));
                    }
                }
                writer.WriteLine(kline.ToCsvLine(downloadTask.DownloadVolume));
                previous = kline;
            }

            return fileName;
        }
    }
}
