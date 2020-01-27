using System.Collections.Generic;
using System.CommandLine.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System.CommandLine
{
    using Context = ApplicationBuilderContext;

    public class ApplicationBuilder : IApplicationBuilder
    {
        private readonly List<Action<Context, IConfigurationBuilder>> _configureConfigurationActions
            = new List<Action<Context, IConfigurationBuilder>>();
        
        private readonly List<Action<Context, IServiceCollection>> _configureServicesActions
            = new List<Action<Context, IServiceCollection>>();
        
        private readonly Context _context = new Context();

        public ApplicationBuilder(RootCommand? command = null)
            : this(new CommandLineBuilder(command)) { }

        public ApplicationBuilder(CommandLineBuilder? builder = null)
        {
            ParserBuilder = builder ?? new CommandLineBuilder();
        }

        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        public CommandLineBuilder ParserBuilder { get; }

        public IApplicationBuilder ConfigureConfiguration(Action<Context, IConfigurationBuilder> configure)
        {
            _configureConfigurationActions.Add(configure);
            return this;
        }

        public IApplicationBuilder ConfigureServices(Action<Context, IServiceCollection> configure)
        {
            _configureServicesActions.Add(configure);
            return this;
        }

        ICommandLineApplication IApplicationBuilder.Build()
        {
            var parser = ParserBuilder.Build();

            var configurationBuilder = new ConfigurationBuilder();
            var serviceCollection = new ServiceCollection();

            foreach (var configure in _configureConfigurationActions)
            {
                configure(_context, configurationBuilder);
            }

            var configuration = configurationBuilder.Build();

            serviceCollection.AddSingleton<IConfiguration>(_ => configuration);

            foreach (var configure in _configureServicesActions)
            {
                configure(_context, serviceCollection);
            }

            var services = serviceCollection.BuildServiceProvider();

            return new CommandLineApplication(services, parser);
        }
    }
}
