using System.CommandLine.Builder;

namespace System.CommandLine
{
    public class CommandLineApplicationBuilder
    {
        public CommandLineApplicationBuilder()
        {
            Builder = new CommandLineBuilder();
        }

        public CommandLineBuilder Builder { get; }

        public CommandLineApplication Build()
        {
            return new CommandLineApplication(Builder);
        }
    }
}
