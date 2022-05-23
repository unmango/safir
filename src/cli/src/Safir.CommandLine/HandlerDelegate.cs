using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Safir.CommandLine;

internal static class HandlerDelegate
{
    public static CommandHandler Create(Func<InvocationContext, IServiceProvider, Task<int>> handler)
        => (context, services) => new(handler(context, services));

    public static CommandHandler Create(Func<IServiceProvider, Task<int>> handler)
        => (_, services) => new(handler(services));

    public static CommandHandler Create(Func<InvocationContext, IServiceProvider, Task> handler)
        => async (context, services) => {
            await handler(context, services);
            return context.ExitCode;
        };

    public static CommandHandler Create(Func<IServiceProvider, Task> handler)
        => async (context, services) => {
            await handler(services);
            return context.ExitCode;
        };

    public static CommandHandler Create(Func<InvocationContext, IServiceProvider, int> handler)
        => (context, services) => new(handler(context, services));

    public static CommandHandler Create(Func<IServiceProvider, int> handler)
        => (_, services) => new(handler(services));

    public static CommandHandler Create(Action<InvocationContext, IServiceProvider> handler)
        => (context, services) => {
            handler(context, services);
            return new(context.ExitCode);
        };

    public static CommandHandler Create(Action<IServiceProvider> handler)
        => (context, services) => {
            handler(services);
            return new(context.ExitCode);
        };
}
