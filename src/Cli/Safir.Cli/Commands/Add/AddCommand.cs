using System.CommandLine;
using System.CommandLine.Builder;
using Safir.CommandLine;

namespace Safir.Cli.Commands.Add
{
    internal class AddCommand : Command
    {
        public AddCommand() : base("add", "") { }

        public static IApplicationBuilder Register(IApplicationBuilder builder)
            => builder.AddCommand<AddCommand>("add", commandBuilder =>
            {
                commandBuilder.UseHandler();
            });
    }
}
