using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Agent.Domain;
using Safir.Agent.Events;

namespace Safir.Agent.Services
{
    internal sealed class DataDirectoryWatcher : IHostedService
    {
        private readonly IOptions<AgentOptions> _options;
        private readonly IDirectory _directory;
        private readonly IPublisher _publisher;
        private readonly ILogger<DataDirectoryWatcher> _logger;
        private FileSystemWatcher? _fileWatcher;

        public DataDirectoryWatcher(
            IOptions<AgentOptions> options,
            IDirectory directory,
            IPublisher publisher,
            ILogger<DataDirectoryWatcher> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _directory = directory ?? throw new ArgumentNullException(nameof(directory));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _logger = logger;
        }
        
        private CancellationTokenSource? TokenSource { get; set; }

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

            _logger.LogTrace("Creating filesystem watcher");
            _fileWatcher = new FileSystemWatcher(root) {
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
            };

            _logger.LogTrace("Assigning filesystem watcher event handlers");
            _fileWatcher.Created += OnCreated;
            _fileWatcher.Changed += OnChanged;
            _fileWatcher.Renamed += OnRenamed;
            _fileWatcher.Deleted += OnDeleted;
            _fileWatcher.Error += OnError;
            
            _logger.LogTrace("Creating cancellation token source");
            TokenSource = new CancellationTokenSource();

            _logger.LogTrace("Finishing data directory watcher start");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping data directory watcher");

            if (TokenSource != null)
            {
                _logger.LogTrace("Disposing token source");
                TokenSource.Dispose();
            }

            if (_fileWatcher == null)
            {
                _logger.LogTrace("No file watcher created, returning");
                return Task.CompletedTask;
            }

            _logger.LogTrace("Removing filesystem watcher event handlers");
            _fileWatcher.Created -= OnCreated;
            _fileWatcher.Changed -= OnChanged;
            _fileWatcher.Renamed -= OnRenamed;
            _fileWatcher.Deleted -= OnDeleted;
            _fileWatcher.Error -= OnError;
            
            _logger.LogTrace("Finishing data directory watcher stop");
            return Task.CompletedTask;
        }

        private async void OnCreated(object sender, FileSystemEventArgs e)
        {
            _logger.LogDebug("Creating file created event");
            var notification = new FileCreated(e.FullPath);
            await SendAsync(this, notification);
        }

        private async void OnChanged(object sender, FileSystemEventArgs e)
        {
            _logger.LogDebug("Creating file changed event");
            var notification = new FileChanged(e.FullPath);
            await SendAsync(this, notification);
        }

        private async void OnRenamed(object sender, FileSystemEventArgs e)
        {
            _logger.LogDebug("Creating file renamed event");
            var notification = new FileRenamed(e.FullPath);
            await SendAsync(this, notification);
        }

        private async void OnDeleted(object sender, FileSystemEventArgs e)
        {
            _logger.LogDebug("Creating file deleted event");
            var notification = new FileDeleted(e.FullPath);
            await SendAsync(this, notification);
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            _logger.LogError(e.GetException(), "Error in file watcher");
        }

        private static Task SendAsync(DataDirectoryWatcher service, INotification notification)
        {
            service._logger.LogDebug("Sending generic file event");
            var token = service.TokenSource?.Token ?? default;
            return service._publisher.Publish(notification, token);
        }
    }
}
