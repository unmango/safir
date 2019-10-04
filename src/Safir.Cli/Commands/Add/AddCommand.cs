using System.CommandLine;
using System.CommandLine.Builder;

namespace Safir.Cli.Commands.Add
{
    internal static class AddCommand
    {
        public const string NAME = "add";
        public const string DESCRIPTION = "";

        public static T Register<T>(T builder) where T : CommandLineBuilder
        {
            var command = new Command(NAME, DESCRIPTION)
            {
                new Argument<string>()
            };

            return builder.AddCommand(command);
        }
    }
}
