using Cli.Services.Configuration;
using Cli.Services.Sources;

namespace Cli.Tests.Helpers
{
    internal record TestConcreteServiceSource(string Name)
        : ServiceSourceBase((SourceType)69, Name)
    {
        public TestConcreteServiceSource() : this("Test")
        {
        }
    }
}
