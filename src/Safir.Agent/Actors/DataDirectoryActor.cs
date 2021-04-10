using System;
using Akka.Actor;
using Akka.Event;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Agent.Domain;

namespace Safir.Agent.Actors
{
    [UsedImplicitly]
    public class DataDirectoryActor : ReceiveActor
    {
        public const string DefaultFilterPattern = "*";
        
        public record List(string FilterPattern = DefaultFilterPattern);

        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly string _root;
        private readonly IDirectory _directory;
        private readonly IOptionsMonitor<AgentOptions> _options;
        private IActorRef? _fileWatcher;

        public DataDirectoryActor(IDirectory directory, IOptionsMonitor<AgentOptions> options, string root)
        {
            _directory = directory ?? throw new ArgumentNullException(nameof(directory));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _root = root;
        }

        protected override void PreStart()
        {
            _logger.Debug("Creating file watcher for data directory");
            var fileWatcherProps = Props.Create<FileWatcherActor>(Context.Parent, _root);
            _fileWatcher = Context.ActorOf(fileWatcherProps, "fileWatcher");
        }

        private void OnList(List message)
        {
            var options = _options.CurrentValue.EnumerationOptions;
            
            _logger.Info("Enumerating data directory");
            var entries = _directory.EnumerateFileSystemEntries(
                _root,
                message.FilterPattern,
                options);
            
            Sender.Tell(entries);
        }
    }
}
