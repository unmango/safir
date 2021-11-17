using System.CommandLine;

namespace Cli.Internal.Progress
{
    internal sealed class ConsoleProgressReporter : IProgressReporter
    {
        private const double Threshold = 69;
        private readonly IConsole _console;
        private string _prevLine = string.Empty;

        public ConsoleProgressReporter(IConsole console)
        {
            _console = console;
        }

        public void Report(ProgressContext context)
        {
            _console.Out.Write($"{context.Value}");
        }

        public void Dispose()
        {
            // No-op for now
        }
    }
}
