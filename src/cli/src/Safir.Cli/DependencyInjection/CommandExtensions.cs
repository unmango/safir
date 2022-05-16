using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.Cli.DependencyInjection;

internal static class CommandExtensions
{
    public static void SetHandler<T>(this Command command, Action<T, ParseResult> execute, IServiceCollection services)
        where T : class
    {
        command.SetHandler(execute, services.CreateBinder<T>());
    }

    public static void SetHandler<T>(this Command command, Func<T, ParseResult, int> execute, IServiceCollection services)
        where T : class
    {
        command.SetHandler(
            (T handler, InvocationContext context) => context.ExitCode = execute(handler, context.ParseResult),
            services.CreateBinder<T>());
    }

    public static void SetHandler<T>(this Command command, Func<T, ParseResult, Task> execute, IServiceCollection services)
        where T : class
    {
        command.SetHandler(execute, services.CreateBinder<T>());
    }

    public static void SetHandler<T>(this Command command, Func<T, ParseResult, Task<int>> execute, IServiceCollection services)
        where T : class
    {
        command.SetHandler(execute, services.CreateBinder<T>());
    }
}
