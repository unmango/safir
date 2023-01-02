using Xunit.Abstractions;
using Xunit.Sdk;

namespace Safir.XUnit.AssemblyFixture;

// https://github.com/xunit/samples.xunit/blob/5dc1d35a63c3394a8678ac466b882576a70f56f6/AssemblyFixtureExample/XunitExtensions/XunitTestCollectionRunnerWithAssemblyFixture.cs

internal class TestCollectionRunner : XunitTestCollectionRunner
{
    private readonly IDictionary<Type, object> _assemblyFixtureMappings;
    private readonly IMessageSink _diagnosticMessageSink;

    public TestCollectionRunner(
        ITestCollection testCollection,
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        ITestCaseOrderer testCaseOrderer,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource,
        IDictionary<Type, object> assemblyFixtureMappings)
        : base(
            testCollection,
            testCases,
            diagnosticMessageSink,
            messageBus,
            testCaseOrderer,
            aggregator,
            cancellationTokenSource)
    {
        _assemblyFixtureMappings = assemblyFixtureMappings;
        _diagnosticMessageSink = diagnosticMessageSink;
    }

    protected override Task<RunSummary> RunTestClassAsync(
        ITestClass testClass,
        IReflectionTypeInfo @class,
        IEnumerable<IXunitTestCase> testCases)
    {
        // Don't want to use .Concat + .ToDictionary because of the possibility of overriding types,
        // so instead we'll just let collection fixtures override assembly fixtures.
        var combinedFixtures = new Dictionary<Type, object>(_assemblyFixtureMappings);
        foreach (var kvp in CollectionFixtureMappings)
            combinedFixtures[kvp.Key] = kvp.Value;

        // We've done everything we need, so let the built-in types do the rest of the heavy lifting
        var classRunner = new XunitTestClassRunner(
            testClass,
            @class,
            testCases,
            _diagnosticMessageSink,
            MessageBus,
            TestCaseOrderer,
            new ExceptionAggregator(Aggregator),
            CancellationTokenSource,
            combinedFixtures);

        return classRunner.RunAsync();
    }
}
