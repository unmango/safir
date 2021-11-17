using System.CommandLine;

namespace Cli.Commands.Service
{
    internal class AllServicesOption : Option
    {
        public AllServicesOption() : base(
            new[] { "--all" },
            "All services")
        { }
    }
}
