using Safir.Cli.Services.Configuration;
using Safir.Cli.Services.Sources;

namespace Safir.Cli.Tests.Helpers;

internal record TestConcreteServiceSource(string Name)
    : ServiceSourceBase((SourceType)69, Name)
{
    public TestConcreteServiceSource() : this("Test")
    {
    }
}