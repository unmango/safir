using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace System.CommandLine
{
    public class CommandLineApplication : ICommandLineApplication, IAsyncDisposable
    {
        private readonly IServiceProvider _services;
        private readonly Parser _parser;
        private readonly ILogger<CommandLineApplication> _logger;

        internal CommandLineApplication(
            IServiceProvider services,
            Parser parser,
            ILogger<CommandLineApplication> logger)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IServiceProvider Services => _services;

        public static IApplicationBuilder CreateDefaultBuilder(CommandLineBuilder? builder = null)
        {
            builder ??= new CommandLineBuilder().UseDefaults();

            return new ApplicationBuilder(builder);
        }

        public void Dispose() => DisposeAsync().GetAwaiter().GetResult();

        public async ValueTask DisposeAsync()
        {
            switch (Services)
            {
                case IAsyncDisposable asyncDisposable:
                    await asyncDisposable.DisposeAsync();
                    break;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }
        }

        Task<int> ICommandLineApplication.RunAsync(string[] args, CancellationToken cancellationToken)
        {
            var console = _services.GetService(typeof(IConsole)) as IConsole;

            return _parser.InvokeAsync(args, console);
        }
    }
}
