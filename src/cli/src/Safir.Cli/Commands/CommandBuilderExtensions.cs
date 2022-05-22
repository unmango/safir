using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Safir.Cli.DependencyInjection;

namespace Safir.Cli.Commands;

// TODO: Calling build to get a binder for the `SetHandler` methods will build configuration
// This is bad because it couples the CLI configuration with the outside world
// For example it can trickle down to unit tests and an invalid config file in the user's
// directory can cause unit test failures across the app
// We need to defer the call to build or come up with another solution so configuration won't
// be built until a command's handler is executing.
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
