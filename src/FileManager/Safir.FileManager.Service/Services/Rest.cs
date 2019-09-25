using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Safir.FileManager.Service.Services
{
    internal class Rest : BackgroundService
    {
        private readonly InvocationContext _context;

        public Rest(InvocationContext context)
        {
            _context = context;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var args = _context.ParseResult.Tokens.Select(x => x.Value).ToArray();
            var host = FileManager.Rest.Program.CreateHostBuilder(args)
                .UseInvocationLifetime(_context)
                .Build();

            return host.RunAsync(stoppingToken);
        }
    }
}
