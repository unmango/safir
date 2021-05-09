using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Safir.Agent.Protos;
using Safir.Messaging;
using Unit = System.Reactive.Unit;

namespace Safir.Agent.Services
{
    internal sealed class FileEventPublisher : IHostedService
    {
        private readonly IFileWatcher _fileWatcher;
        private readonly IPublisher _publisher;
        private readonly ILogger<FileEventPublisher> _logger;
        private List<IDisposable>? _subscriptions;

        public FileEventPublisher(IFileWatcher fileWatcher, IPublisher publisher, ILogger<FileEventPublisher> logger)
        {
            _fileWatcher = fileWatcher ?? throw new ArgumentNullException(nameof(fileWatcher));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting file event publisher");
            _subscriptions = new() {
                _fileWatcher.Created.SelectMany(OnCreated).Subscribe(),
                _fileWatcher.Changed.SelectMany(OnChanged).Subscribe(),
                _fileWatcher.Deleted.SelectMany(OnDeleted).Subscribe(),
                _fileWatcher.Renamed.SelectMany(OnRenamed).Subscribe(),
            };

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping file event publisher");
            _subscriptions?.ForEach(x => x.Dispose());

            return Task.CompletedTask;
        }

        private Task<Unit> OnCreated(FileSystemEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Publishing created event");
            var @event = new FileCreated { Path = eventArgs.Name };
            return Publish(_publisher, @event, cancellationToken);
        }

        private Task<Unit> OnChanged(FileSystemEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Publishing changed event");
            var @event = new FileChanged { Path = eventArgs.Name };
            return Publish(_publisher, @event, cancellationToken);
        }

        private Task<Unit> OnDeleted(FileSystemEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Publishing deleted event");
            var @event = new FileDeleted { Path = eventArgs.Name };
            return Publish(_publisher, @event, cancellationToken);
        }

        private Task<Unit> OnRenamed(RenamedEventArgs eventArgs, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Publishing renamed event");
            var @event = new FileRenamed { Path = eventArgs.Name, OldPath = eventArgs.OldName };
            return Publish(_publisher, @event, cancellationToken);
        }

        private static async Task<Unit> Publish<T>(
            IPublisher publisher,
            T message,
            CancellationToken cancellationToken)
            where T : IEvent
        {
            await publisher.Publish<T>(message, cancellationToken);
            return Unit.Default;
        }
    }
}
