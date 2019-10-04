using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace System.CommandLine
{
    public class CommandLineApplication : ICommandLineApplication
    {
        private readonly Parser _parser;

        public CommandLineApplication(IServiceProvider services, Parser parser)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public IServiceProvider Services { get; }

        public Task<int> RunAsync(string[] args, CancellationToken cancellationToken = default)
        {
            var parseResult = _parser.Parse(args);
            var console = Services.GetService(typeof(IConsole)) as IConsole;

            return _parser.InvokeAsync(parseResult, console);
        }
    }
}
