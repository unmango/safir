using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DependencyInjection;
using Safir.Agent.Actors;

namespace Safir.Agent.Services
{
    internal sealed class AkkaDataManager : IDataManager
    {
        private readonly IActorRef _dataManager;

        public AkkaDataManager(IAkkaSystem akkaSystem)
        {
            var system = akkaSystem.System ?? throw new ArgumentNullException(nameof(akkaSystem));
            var serviceProvider = ServiceProvider.For(system);
            var props = serviceProvider.Props<DataManagerActor>();
            _dataManager = system.ActorOf(props, "data");
        }

        public Task<IEnumerable<string>> ListAsync(
            string? filterPattern = null,
            CancellationToken cancellationToken = default)
        {
            DataDirectoryActor.List message = string.IsNullOrWhiteSpace(filterPattern)
                ? new()
                : new(filterPattern);

            return _dataManager.Ask<IEnumerable<string>>(message, cancellationToken);
        }
    }
}
