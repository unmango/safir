using System.Reflection;
using JetBrains.Annotations;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Safir.XUnit.AssemblyFixture;

// https://github.com/xunit/samples.xunit/blob/5dc1d35a63c3394a8678ac466b882576a70f56f6/AssemblyFixtureExample/XunitExtensions/XunitTestFrameworkWithAssemblyFixture.cs

[PublicAPI]
public class TestFramework : XunitTestFramework
{
    public TestFramework(IMessageSink messageSink) : base(messageSink) { }

    protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
        => new TestFrameworkExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
}
