using Safir.Cli.EndToEndTests;
using Safir.XUnit.AssemblyFixture;

[assembly: TestFramework("Safir.XUnit.AssemblyFixture.TestFramework", "Safir.XUnit.AssemblyFixture")]
[assembly: AssemblyFixture<CliFixture>]
