using System.CommandLine.Builder;

namespace System.CommandLine
{
    public class CommandLineApplicationBuilder
    {
        public CommandLineApplicationBuilder()
            : this(new CommandLineBuilder())
        { }

        public CommandLineApplicationBuilder(CommandLineBuilder builder)
        {
            Builder = builder;
        }

        public CommandLineBuilder Builder { get; }

        public CommandLineApplication Build()
        {
            return new CommandLineApplication(Builder);
        }

        public CommandLineApplicationBuilder UseServiceProviderFactory(Func<IServiceProvider> factory)
        {
            return this;
        }
    }
}
