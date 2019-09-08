using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace System.CommandLine
{
    public class CommandLineApplication
    {
        private readonly Func<string[], CancellationToken, Task<int>> _invoke;

        public CommandLineApplication([NotNull] CommandLineBuilder builder)
            : this(builder?.Build() ?? throw new ArgumentNullException(nameof(builder))) { }

        public CommandLineApplication([NotNull] Parser parser)
        {
            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            _invoke = (args, _) => parser.InvokeAsync(args);
        }

        public IServiceProvider Services { get; }

        public Task<int> RunAsync(string[] args, CancellationToken cancellationToken = default)
        {
            return _invoke(args, cancellationToken);
        }

        public static CommandLineApplicationBuilder CreateDefaultBuilder()
        {
            var builder = new CommandLineApplicationBuilder();

            return builder;
        }
    }
}
