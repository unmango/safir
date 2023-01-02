using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Safir.XUnit.AssemblyFixture;

// https://github.com/xunit/samples.xunit/blob/5dc1d35a63c3394a8678ac466b882576a70f56f6/AssemblyFixtureExample/XunitExtensions/XunitTestAssemblyRunnerWithAssemblyFixture.cs

internal class TestAssemblyRunner : XunitTestAssemblyRunner
{
    private readonly IDictionary<Type, object> _assemblyFixtureMappings = new Dictionary<Type, object>();

    public TestAssemblyRunner(
        ITestAssembly testAssembly,
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink,
        IMessageSink executionMessageSink,
        ITestFrameworkExecutionOptions executionOptions)
        : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions) { }

    protected override async Task AfterTestAssemblyStartingAsync()
    {
        // Let everything initialize
        await base.AfterTestAssemblyStartingAsync();

        // Go find all the AssemblyFixtureAttributes adorned on the test assembly
        await Aggregator.RunAsync(async () => {
            var fixturesAttrs = ((IReflectionAssemblyInfo)TestAssembly.Assembly).Assembly
                .GetCustomAttributes(typeof(AssemblyFixtureAttribute), false)
                .Cast<AssemblyFixtureAttribute>()
                .ToList();

            // Instantiate all the fixtures
            foreach (var fixtureAttr in fixturesAttrs) {
                var fixture = Activator.CreateInstance(fixtureAttr.FixtureType)
                              ?? throw new InvalidOperationException("Unable to create fixture");

                if (fixture is IAsyncLifetime lifetime)
                    await lifetime.InitializeAsync();

                _assemblyFixtureMappings[fixtureAttr.FixtureType] = fixture;
            }
        });
    }

    protected override async Task BeforeTestAssemblyFinishedAsync()
    {
        foreach (var disposable in _assemblyFixtureMappings.Values.OfType<IAsyncLifetime>())
            await Aggregator.RunAsync(disposable.DisposeAsync);

        // Make sure we clean up everybody who is disposable, and use Aggregator.Run to isolate Dispose failures
        foreach (var disposable in _assemblyFixtureMappings.Values.OfType<IDisposable>())
            Aggregator.Run(disposable.Dispose);

        await base.BeforeTestAssemblyFinishedAsync();
    }

    protected override Task<RunSummary> RunTestCollectionAsync(
        IMessageBus messageBus,
        ITestCollection testCollection,
        IEnumerable<IXunitTestCase> testCases,
        CancellationTokenSource cancellationTokenSource)
        => new TestCollectionRunner(
                testCollection,
                testCases,
                DiagnosticMessageSink,
                messageBus,
                TestCaseOrderer,
                new ExceptionAggregator(Aggregator),
                cancellationTokenSource,
                _assemblyFixtureMappings)
            .RunAsync();
}
