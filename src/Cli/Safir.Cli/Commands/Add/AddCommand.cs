using System.CommandLine;
using System.CommandLine.Builder;

namespace Safir.Cli.Commands.Add
{
    internal class AddCommand
    {
        public static IApplicationBuilder Register(IApplicationBuilder builder)
        {
            return builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
