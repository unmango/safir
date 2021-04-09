using System;
using Akka.Actor;
using Akka.Event;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;

namespace Safir.Agent.Actors
{
    [UsedImplicitly]
    internal sealed class DataManagerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly IServiceScope _serviceScope;
        private readonly IServiceProvider _services;
        private readonly IOptionsMonitor<AgentOptions> _options;
        private IActorRef? _fileWatcher;
        
        public DataManagerActor(IServiceProvider services)
        {
            _serviceScope = services.CreateScope();
            _services = _serviceScope.ServiceProvider;
            _options = _services.GetRequiredService<IOptionsMonitor<AgentOptions>>();

            Receive<DirectoryValidator.ValidationError>(OnValidationError);
            Receive<DirectoryValidator.ValidationSuccess>(OnValidationSuccess);
        }

        protected override void PreStart()
        {
            var options = _options.CurrentValue;
            var validator = Context.ActorOf<DirectoryValidator>();
            validator.Tell(options.DataDirectory);
        }

        protected override void PostStop()
        {
            _serviceScope.Dispose();
        }

        private void OnValidationError(DirectoryValidator.ValidationError message)
        {
            _logger.Error("Data directory is invalid: {Message}", message.Message);
        }

        private void OnValidationSuccess(DirectoryValidator.ValidationSuccess message)
        {
            _logger.Debug("Data directory validated");
            var directory = _options.CurrentValue.DataDirectory!;
            
            _logger.Debug("Creating file watcher for data directory");
            var fileWatcherProps = Props.Create<FileWatcherActor>(Self, directory);
            _fileWatcher = Context.ActorOf(fileWatcherProps, "fileWatcher");
        }
    }
}
