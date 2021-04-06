using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Safir.Agent.Services
{
    internal sealed class AkkaService : IHostedService
    {
        private readonly IServiceProvider _services;
        private ActorSystem? _system;

        public AkkaService(IServiceProvider services)
        {
            _services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var hocon = await File.ReadAllTextAsync("app.hocon", cancellationToken);
            var configuration = ConfigurationFactory.ParseString(hocon);
            var bootstrap = BootstrapSetup.Create().WithConfig(configuration);
            var di = ServiceProviderSetup.Create(_services);
            
            _system = ActorSystem.Create("SafirAgent", bootstrap.And(di));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // https://github.com/akkadotnet/akka.net/blob/40915081652e36f3b9ab3b9ace002d1cbb4e0151/src/examples/AspNetCore/Samples.Akka.AspNetCore/Actors/AkkaService.cs#L58
            // theoretically, shouldn't even need this - will be invoked automatically via CLR exit hook
            // but it's good practice to actually terminate IHostedServices when ASP.NET asks you to
            await CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }
}
