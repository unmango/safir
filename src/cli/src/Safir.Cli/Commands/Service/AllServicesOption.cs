using System.CommandLine;

namespace Safir.Cli.Commands.Service
{
    internal class AllServicesOption : Option
    {
        public AllServicesOption() : base(
            new[] { "--all" },
            "All services")
        { }
    }
}
