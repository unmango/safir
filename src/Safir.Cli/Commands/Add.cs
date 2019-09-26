using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Safir.Cli.Commands
{
    internal static class Add
    {
        public static readonly string Name = "add";
        public static readonly string Description = "";

        public static T UseAddCommand<T>(this T builder)
            where T : CommandLineBuilder
            => builder.AddCommand(Name, Description, Configure);

        private static void Configure(CommandBuilder builder)
        {
            builder.UseHandler(CommandHandler.Create<IConsole>(Execute));
        }

        private static Task Execute(IConsole console)
        {
            console.Out.WriteLine("In Add execute");

            return Task.CompletedTask;
        }
    }
}
