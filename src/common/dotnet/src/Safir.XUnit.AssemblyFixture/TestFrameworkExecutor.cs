using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Safir.XUnit.AssemblyFixture;

// https://github.com/xunit/samples.xunit/blob/5dc1d35a63c3394a8678ac466b882576a70f56f6/AssemblyFixtureExample/XunitExtensions/XunitTestFrameworkExecutorWithAssemblyFixture.cs

internal class TestFrameworkExecutor : XunitTestFrameworkExecutor
{
    public TestFrameworkExecutor(
        AssemblyName assemblyName,
        ISourceInformationProvider sourceInformationProvider,
        IMessageSink diagnosticMessageSink)
        : base(assemblyName, sourceInformationProvider, diagnosticMessageSink) { }

    protected override async void RunTestCases(
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink executionMessageSink,
        ITestFrameworkExecutionOptions executionOptions)
    {
        using var assemblyRunner = new TestAssemblyRunner(
            TestAssembly,
            testCases,
            DiagnosticMessageSink,
            executionMessageSink,
            executionOptions);

        await assemblyRunner.RunAsync();
    }
}
