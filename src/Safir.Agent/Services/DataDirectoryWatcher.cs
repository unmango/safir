using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Agent.Domain;

namespace Safir.Agent.Services
{
    internal sealed class DataDirectoryWatcher : IHostedService, IFileWatcher
    {
        private static readonly IObservable<FileSystemEventArgs> _emptyObservable =
            Observable.Empty<FileSystemEventArgs>();

        private readonly IOptions<AgentOptions> _options;
        private readonly IDirectory _directory;
        private readonly ILogger<DataDirectoryWatcher> _logger;
        private FileSystemWatcher? _fileWatcher;

        public DataDirectoryWatcher(
            IOptions<AgentOptions> options,
            IDirectory directory,
            ILogger<DataDirectoryWatcher> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _directory = directory ?? throw new ArgumentNullException(nameof(directory));
            _logger = logger;
        }

        public IObservable<FileSystemEventArgs> Created { get; private set; } = _emptyObservable;

        public IObservable<FileSystemEventArgs> Changed { get; private set; } = _emptyObservable;

        public IObservable<FileSystemEventArgs> Deleted { get; private set; } = _emptyObservable;

        public IObservable<RenamedEventArgs> Renamed { get; private set; } = Observable.Empty<RenamedEventArgs>();

        public IObservable<ErrorEventArgs> Error { get; private set; } = Observable.Empty<ErrorEventArgs>();

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting data directory watcher");

            var root = _options.Value.DataDirectory;
            if (string.IsNullOrWhiteSpace(root))
            {
                _logger.LogInformation("No data directory set");
                return Task.CompletedTask;
            }

            if (!_directory.Exists(root))
            {
                _logger.LogError("Data directory does not exist");
                return Task.CompletedTask;
            }

            _logger.LogTrace("Creating filesystem watcher for {Directory}", root);
            _fileWatcher = new FileSystemWatcher(root) {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };

            _fileWatcher.Created += (sender, args) => _logger.LogTrace("File created");
            _fileWatcher.Deleted += (sender, args) => _logger.LogTrace("File created");

            _logger.LogTrace("Assigning filesystem watcher event handlers");
            CreateObservablesFromEvents(_fileWatcher);

            _logger.LogTrace("Finishing data directory watcher start");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping data directory watcher");

            if (_fileWatcher == null)
            {
                _logger.LogTrace("No file watcher created, returning");
                return Task.CompletedTask;
            }

            _logger.LogTrace("Disposing file watcher");
            _fileWatcher.Dispose();

            _logger.LogTrace("Finishing data directory watcher stop");
            return Task.CompletedTask;
        }

        private void CreateObservablesFromEvents(FileSystemWatcher watcher)
        {
            _logger.LogTrace("Creating [Created] event observable");
            Created = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    x => watcher.Created += x,
                    x => watcher.Created -= x)
                .Select(x => x.EventArgs);

            _logger.LogTrace("Creating [Changed] event observable");
            Changed = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    x => watcher.Changed += x,
                    x => watcher.Changed -= x)
                .Select(x => x.EventArgs);

            _logger.LogTrace("Creating [Deleted] event observable");
            Deleted = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    x => watcher.Deleted += x,
                    x => watcher.Deleted -= x)
                .Select(x => x.EventArgs);

            _logger.LogTrace("Creating [Renamed] event observable");
            Renamed = Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                    x => watcher.Renamed += x,
                    x => watcher.Renamed -= x)
                .Select(x => x.EventArgs);

            _logger.LogTrace("Creating [Error] event observable");
            Error = Observable.FromEventPattern<ErrorEventHandler, ErrorEventArgs>(
                    x => watcher.Error += x,
                    x => watcher.Error -= x)
                .Select(x => x.EventArgs);
        }
    }
}
