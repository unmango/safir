using System.Collections.Generic;
using System.CommandLine.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System.CommandLine
{
    using Context = CommandLineApplicationBuilderContext;

    public class CommandLineApplicationBuilder : ICommandLineApplicationBuilder
    {
        private readonly List<Action<Context, IConfigurationBuilder>> _configureConfigurationActions
            = new List<Action<Context, IConfigurationBuilder>>();
        
        private readonly List<Action<Context, IServiceCollection>> _configureServicesActions
            = new List<Action<Context, IServiceCollection>>();
        
        private readonly Context _context = new Context();

        public CommandLineApplicationBuilder() : this(new CommandLineBuilder())
        { }

        public CommandLineApplicationBuilder(CommandLineBuilder builder)
        {
            ParserBuilder = builder;
        }

        public CommandLineBuilder ParserBuilder { get; }

        public ICommandLineApplicationBuilder ConfigureConfiguration(Action<Context, IConfigurationBuilder> configure)
        {
            _configureConfigurationActions.Add(configure);
            return this;
        }

        public ICommandLineApplicationBuilder ConfigureServices(Action<Context, IServiceCollection> configure)
        {
            _configureServicesActions.Add(configure);
            return this;
        }

        public ICommandLineApplicationBuilder UseServiceProviderFactory<T>(Func<Context, IServiceProviderFactory<T>> factory)
        {
            throw new NotImplementedException();
        }

        ICommandLineApplication ICommandLineApplicationBuilder.Build()
        {
            var parser = ParserBuilder.Build();

            return new CommandLineApplication(null, parser);
        }
    }
}
