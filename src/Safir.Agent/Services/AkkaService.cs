using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Safir.Agent.Actors;

namespace Safir.Agent.Services
{
    using ServiceProvider = Akka.DependencyInjection.ServiceProvider;
    
    internal sealed class AkkaService : IHostedService, IAkkaSystem
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<AkkaService> _logger;
        private ActorSystem? _system;

        public AkkaService(IServiceProvider services, ILogger<AkkaService> logger)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _logger = logger;
        }

        public ActorSystem System => _system ?? throw new InvalidOperationException("System has not been initialized");

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting akka service");
            
            _logger.LogDebug("Reading app.hocon");
            var hocon = await ReadAppHoconAsync(cancellationToken);
            
            _logger.LogDebug("Creating akka configuration");
            var configuration = ConfigurationFactory.ParseString(hocon);
            var bootstrap = BootstrapSetup.Create().WithConfig(configuration);
            var di = ServiceProviderSetup.Create(_services);
            
            _logger.LogDebug("Creating safir agent actor system");
            _system = ActorSystem.Create("SafirAgent", bootstrap.And(di));

            var serviceProvider = ServiceProvider.For(_system);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // https://github.com/akkadotnet/akka.net/blob/40915081652e36f3b9ab3b9ace002d1cbb4e0151/src/examples/AspNetCore/Samples.Akka.AspNetCore/Actors/AkkaService.cs#L58
            // theoretically, shouldn't even need this - will be invoked automatically via CLR exit hook
            // but it's good practice to actually terminate IHostedServices when ASP.NET asks you to
            await CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }

        private Task<string> ReadAppHoconAsync(CancellationToken cancellationToken)
        {
            var environment = _services.GetRequiredService<IHostEnvironment>();
            var file = environment.EnvironmentName switch {
                // Environments.Development => "app.Development.hocon", // Maybe in C# 10?
                "Development" => "app.Development.hocon",
                _ => "app.hocon",
            };

            return File.ReadAllTextAsync(file, cancellationToken);
        }
    }
}
