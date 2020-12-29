using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Threading.Tasks;
using Cli.Commands;
using Cli.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;
using static System.Environment;

namespace Cli
{
    internal static class Program
    {
        private const string ConfigDirectoryKey = "config:directory";
        private const string ConfigFileKey = "config:file";
        private const string ConfigExistsKey = "config:exists";
        
        private static readonly Option _debugOption = new(
            new[] { "--debug", "-d" },
            "Write debug information to the console");

        private static async Task<int> Main(string[] args) => await CreateBuilder()
            .AddCommand(new ServiceCommand())
            .UseHost(host => host
                .AddServiceCommand()
                .ConfigureHostConfiguration(configuration => {
                    configuration.AddStaticConfiguration();
                })
                .ConfigureAppConfiguration((context, configuration) => {
                    configuration.AddEnvironmentVariables("SAFIR_");
                    configuration.AddJsonFile(
                        context.Configuration[ConfigFileKey],
                        optional: true,
                        reloadOnChange: true);
                })
                .ConfigureServices((context, services) => {
                    services.AddLogging();
                    services.AddOptions();
                    
                    services.Configure<Options>(context.Configuration);
                    services.Configure<Config>(context.Configuration.GetSection("config"));
                })
                .ConfigureLogging((context, builder) => {
                    var configDir = context.Configuration[ConfigDirectoryKey];
                    var logDir = Path.Combine(configDir, "logs");
                    var logFile = Path.Combine(logDir, "log.json");

                    var configuration = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .WriteTo.Async(x => x.File(new CompactJsonFormatter(), logFile));

                    if (context.Properties[typeof(InvocationContext)] is InvocationContext invocation
                        && invocation.ParseResult.HasOption(_debugOption))
                    {
                        configuration.WriteTo.Console();
                    }

                    builder.AddSerilog(configuration.CreateLogger(), dispose: true);
                }))
            .Build()
            .InvokeAsync(args);

        private static CommandLineBuilder CreateBuilder() => new CommandLineBuilder()
            .UseHelpForEmptyCommands()
            .AddGlobalOption(_debugOption)
            .UseDefaults();

        private static IConfigurationBuilder AddStaticConfiguration(this IConfigurationBuilder builder)
        {
            var configDir = Path.Join(
                GetFolderPath(SpecialFolder.UserProfile),
                ".safir");

            if (!Directory.Exists(configDir))
                Directory.CreateDirectory(configDir);

            var configFile = Path.Join(configDir, "config.json");

            return builder.AddInMemoryCollection(new Dictionary<string, string> {
                { ConfigDirectoryKey, configDir },
                { ConfigFileKey, configFile },
                { ConfigExistsKey, File.Exists(configFile).ToString() }
            });
        }
    }
}
