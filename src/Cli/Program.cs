using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Threading.Tasks;
using Cli.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Compact;
using static System.Environment;

namespace Cli
{
    internal static class Program
    {
        private static readonly Option _debugOption = new(
            new[] { "--debug", "-d" },
            "Write debug information to the console");

        private static async Task<int> Main(string[] args) => await CreateBuilder()
            .UseHost(host => host
                .ConfigureHostConfiguration(configuration => configuration
                    .AddInMemoryCollection(GetStaticConfiguration()))
                .ConfigureAppConfiguration((context, configuration) => configuration
                    .AddEnvironmentVariables("SAFIR_")
                    .AddJsonFile(context.Configuration["config:file"], optional: true, reloadOnChange: true))
                .ConfigureServices((context, services) => services
                    .AddLogging(logBuilder => {
                        var configDir = context.Configuration["config:directory"];
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

                        logBuilder.AddSerilog(configuration.CreateLogger(), dispose: true);
                    })
                    .Configure<Options>(context.Configuration)))
            .Build()
            .InvokeAsync(args);

        private static CommandLineBuilder CreateBuilder() => new CommandLineBuilder()
            .UseHelpForEmptyCommands()
            .AddGlobalOption(_debugOption)
            .UseDefaults();

        private static Dictionary<string, string> GetStaticConfiguration()
        {
            var configDir = Path.Join(
                GetFolderPath(SpecialFolder.UserProfile),
                ".safir");

            if (!Directory.Exists(configDir))
                Directory.CreateDirectory(configDir);

            var configFile = Path.Join(configDir, "config.json");

            return new Dictionary<string, string> {
                { "config:directory", configDir },
                { "config:file", configFile },
                { "config:exists", File.Exists(configFile).ToString() }
            };
        }
    }
}
