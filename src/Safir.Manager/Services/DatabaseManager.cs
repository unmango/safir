using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Safir.Manager.Data;

namespace Safir.Manager.Services
{
    internal class DatabaseManager : IHostedService
    {
        private readonly IServiceProvider _services;

        public DatabaseManager(IServiceProvider services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ManagerContext>();

            if (!await context.Database.CanConnectAsync(cancellationToken))
            {
                throw new InvalidOperationException("Couldn't connect to manager database");
            }

            var pending = await context.Database.GetPendingMigrationsAsync(cancellationToken);
            if (pending.Any())
            {
                await context.Database.MigrateAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}