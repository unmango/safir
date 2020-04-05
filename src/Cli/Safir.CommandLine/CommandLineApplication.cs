using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace System.CommandLine
{
    public class CommandLineApplication : ICommandLineApplication, IAsyncDisposable
    {
        private readonly Parser _parser;
        private readonly ILogger<CommandLineApplication> _logger;

        internal CommandLineApplication(
            IServiceProvider services,
            Parser parser,
            ILogger<CommandLineApplication> logger)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IServiceProvider Services { get; }

        public static IApplicationBuilder CreateDefaultBuilder(RootCommand? command = null)
            => new ApplicationBuilder(command)
            .ConfigureCommands(builder =>
            {
                builder.UseDefaults();
            });

        public void Dispose() => DisposeAsync().GetAwaiter().GetResult();

        public async ValueTask DisposeAsync()
        {
            Logger.Disposing(_logger);

            switch (Services)
            {
                case IAsyncDisposable asyncDisposable:
                    Logger.AsyncDisposal(_logger);
                    await asyncDisposable.DisposeAsync();
                    break;
                case IDisposable disposable:
                    Logger.SyncDisposal(_logger);
                    disposable.Dispose();
                    break;
            }
        }

        Task<int> ICommandLineApplication.RunAsync(string[] args, CancellationToken cancellationToken)
        {
            var console = Services.GetService(typeof(IConsole)) as IConsole;

            Logger.Invoking(_logger, args);

            return _parser.InvokeAsync(args, console);
        }

        private static class Logger
        {
            private static readonly Action<ILogger, string[], Exception?> _invoking = LoggerMessage.Define<string[]>(
                LogLevel.Information,
                new EventId(1, "invoking"),
                "Invoking parser with args [{args}]");

            private static readonly Action<ILogger, Exception?> _disposing = LoggerMessage.Define(
                LogLevel.Information,
                new EventId(2, "disposing"),
                "Disposing the command line application");

            private static readonly Action<ILogger, Exception?> _syncDisposal = LoggerMessage.Define(
                LogLevel.Trace,
                new EventId(3, "synchronous_disposal"),
                "Service collection was disposed synchronously");

            private static readonly Action<ILogger, Exception?> _asyncDisposal = LoggerMessage.Define(
                LogLevel.Trace,
                new EventId(4, "asynchronous_disposal"),
                "Service collection was disposed asynchronously");

            public static void AsyncDisposal(ILogger<CommandLineApplication> logger)
                => _asyncDisposal(logger, null);

            public static void Disposing(ILogger<CommandLineApplication> logger)
                => _disposing(logger, null);

            public static void Invoking(ILogger<CommandLineApplication> logger, string[] args)
                => _invoking(logger, args, null);

            public static void SyncDisposal(ILogger<CommandLineApplication> logger)
                => _syncDisposal(logger, null);
        }
    }
}
