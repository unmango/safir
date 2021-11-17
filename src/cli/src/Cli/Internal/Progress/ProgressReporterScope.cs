namespace Cli.Internal.Progress
{
    internal class ProgressReporterScope : IProgressScope
    {
        private readonly IProgressReporter _reporter;
        private ProgressScope _scope;

        public ProgressReporterScope(IProgressReporter reporter, string name)
        {
            _reporter = reporter;
            _scope = ProgressScope.Named(name);
        }

        public void Dispose()
        {
            _scope = ProgressScope.Empty;
        }

        public void Report(ProgressContext context)
        {
            // TODO: This is basically a no-op. Scope should do some formatting
            // Maybe add a scope formatting handler in the pipeline... seems like over-engineering
            _reporter.Report(context with { Scope = _scope });
        }
    }
}
