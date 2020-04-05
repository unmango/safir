using System.Collections.Generic;
using System.CommandLine.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System.CommandLine
{
    using Context = ApplicationBuilderContext;

    public class ApplicationBuilder : IApplicationBuilder
    {
        private readonly RootCommand? _rootCommand;

        private readonly List<Action<Context, CommandLineBuilder>> _configureCommandActions
            = new List<Action<Context, CommandLineBuilder>>();

        private readonly List<Action<Context, IConfigurationBuilder>> _configureConfigurationActions
            = new List<Action<Context, IConfigurationBuilder>>();
        
        private readonly List<Action<Context, IServiceCollection>> _configureServicesActions
            = new List<Action<Context, IServiceCollection>>();

        public ApplicationBuilder(RootCommand? command = null)
        {
            _rootCommand = command;
        }

        public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

        public IApplicationBuilder ConfigureCommands(Action<Context, CommandLineBuilder> configure)
        {
            _configureCommandActions.Add(configure);
            return this;
        }

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
            var context = new Context(Properties);
            var commandLineBuilder = new CommandLineBuilder(_rootCommand);
            var configurationBuilder = new ConfigurationBuilder();
            var serviceCollection = new ServiceCollection();

            foreach (var configure in _configureConfigurationActions)
            {
                configure(context, configurationBuilder);
            }

            foreach (var configure in _configureCommandActions)
            {
                configure(context, commandLineBuilder);
            }

            var parser = commandLineBuilder.Build();
            var configuration = configurationBuilder.Build();

            serviceCollection.AddSingleton(context);
            // register configuration as factory to make it dispose with the service provider
            serviceCollection.AddSingleton<IConfiguration>(_ => configuration);
            serviceCollection.AddSingleton(parser);
            serviceCollection.AddSingleton<ICommandLineApplication, CommandLineApplication>();
            serviceCollection.AddLogging();

            foreach (var configure in _configureServicesActions)
            {
                configure(context, serviceCollection);
            }

            var services = serviceCollection.BuildServiceProvider();

            // resolve configuration explicitly once to mark it as resolved within the
            // service provider, ensuring it will be properly disposed with the provider
            _ = services.GetService<IConfiguration>();

            return services.GetRequiredService<ICommandLineApplication>();
        }
    }
}
