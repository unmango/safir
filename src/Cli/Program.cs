using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Compact;
using static System.Environment;

namespace Cli
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var debugOption = new Option(
                new[] { "--debug", "-d" },
                "Write debug information to the console");

            var parseResult = new CommandLineBuilder()
                .AddGlobalOption(debugOption)
                .UseDefaults()
                .Build()
                .Parse(args);

            var configDir = Path.Join(
                GetFolderPath(SpecialFolder.UserProfile),
                ".safir");

            if (!Directory.Exists(configDir))
                Directory.CreateDirectory(configDir);

            var configFile = Path.Join(configDir, "config.json");

            Dictionary<string, string> staticConfig = new() {
                { "config:file", configFile },
                { "config:exists", File.Exists(configFile).ToString() }
            };
            
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(staticConfig)
                .AddEnvironmentVariables("SAFIR_")
                .AddJsonFile(configFile, optional: true, reloadOnChange: true)
                .AddCommandLine(args)
                .Build();

            var logDir = Path.Combine(configDir, "logs");
            var logFile = Path.Combine(logDir, "log.json");

            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Async(x => x.File(new CompactJsonFormatter(), logFile));

            if (parseResult.HasOption(debugOption))
                loggerConfiguration.WriteTo.Console();

            Log.Logger = loggerConfiguration.CreateLogger();

            var services = new ServiceCollection()
                .AddLogging(logBuilder => {
                    logBuilder.AddSerilog(dispose: true);
                })
                .Configure<Options>(configuration)
                .AddSingleton<IConsole, SystemConsole>()
                .BuildServiceProvider();

            var console = services.GetService<IConsole>();

            return await parseResult.InvokeAsync(console);
        }
    }
}
