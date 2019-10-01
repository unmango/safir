using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Safir.Cli.Commands
{
    internal class Add
    {
        public static readonly string Name = "add";
        public static readonly string Description = "";

        public static T Register<T>(T builder) where T : CommandLineBuilder
        {
            return builder.AddCommand(Name, Description, Configure);
        }

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
