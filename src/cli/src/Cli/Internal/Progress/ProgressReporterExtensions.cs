namespace Cli.Internal.Progress
{
    internal static class ProgressReporterExtensions
    {
        public static void Report(this IProgressReporter reporter, string text)
        {
            reporter.Report(new ProgressContext(text));
        }
    }
}
