using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Safir.Cli.DependencyInjection;

namespace Safir.Cli.Commands;

internal static class CommandBuilderExtensions
{
    public static CommandBuilder ConfigureServices(this CommandBuilder builder, Action<IServiceCollection> configure)
        => builder.ConfigureServices((_, services) => configure(services));

    public static void SetHandler<T>(this CommandBuilder builder, Command command, Action<T, ParseResult> execute)
        where T : class
        => command.SetHandler(execute, builder.Build().AddSingleton<T>());

    public static void SetHandler<T>(this CommandBuilder builder, Command command, Func<T, ParseResult, int> execute)
        where T : class
        => command.SetHandler(execute, builder.Build().AddSingleton<T>());

    public static void SetHandler<T>(this CommandBuilder builder, Command command, Func<T, ParseResult, Task> execute)
        where T : class
        => command.SetHandler(execute, builder.Build().AddSingleton<T>());

    public static void SetHandler<T>(this CommandBuilder builder, Command command, Func<T, ParseResult, Task<int>> execute)
        where T : class
        => command.SetHandler(execute, builder.Build().AddSingleton<T>());
}
