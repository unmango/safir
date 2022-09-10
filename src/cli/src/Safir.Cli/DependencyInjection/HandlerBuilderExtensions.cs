using Microsoft.Extensions.Configuration;
using Safir.Agent.Client.DependencyInjection;
using Safir.Cli.Configuration;
using Safir.CommandLine;

namespace Safir.Cli.DependencyInjection;

internal static class HandlerBuilderExtensions
{
    public static IHandlerBuilder AddConfiguredAgents(this IHandlerBuilder builder) => builder
        .ConfigureServices((context, services) => {
            var options = new SafirOptions();
            context.Configuration.Bind(options);

            if (options.Agents is null) {
                services.AddSafirAgentClient();
                return;
            }

            foreach (var agent in options.Agents) {
                services.AddSafirAgentClient(agent.Name, agentOptions => {
                    agentOptions.Address = new(agent.Uri);
                });
            }
        });

    public static IHandlerBuilder UseSafirDefaults(this IHandlerBuilder builder) => builder
        .ConfigureAppConfiguration(configurationBuilder => {
            configurationBuilder.AddSafirCliDefault();
        })
        .ConfigureServices(services => {
            services.AddSafirCliCore();
            services.AddSafirOptions();
        });
}
