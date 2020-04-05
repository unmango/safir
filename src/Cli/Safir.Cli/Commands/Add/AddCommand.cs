using System.CommandLine;
using System.CommandLine.Builder;
using Safir.CommandLine;

namespace Safir.Cli.Commands.Add
{
    internal class AddCommand
    {
        public static IApplicationBuilder Register(IApplicationBuilder builder)
            => builder.AddCommand("add", commandBuilder =>
            {

            });
    }
}
