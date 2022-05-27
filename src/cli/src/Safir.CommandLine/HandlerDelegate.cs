using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

internal static class HandlerDelegate
{
    public static CommandHandler Create(Func<InvocationContext, IServiceProvider, Task<int>> handler)
        => (context, services) => new(handler(context, services));

    public static CommandHandler Create<T>(Func<InvocationContext, T, Task<int>> handler)
        where T : notnull
        => (context, services) => new(handler(context, services.GetRequiredService<T>()));

    public static CommandHandler Create(Func<IServiceProvider, Task<int>> handler)
        => (_, services) => new(handler(services));

    public static CommandHandler Create<T>(Func<T, Task<int>> handler)
        where T : notnull
        => (_, services) => new(handler(services.GetRequiredService<T>()));

    public static CommandHandler Create(Func<InvocationContext, IServiceProvider, Task> handler)
        => async (context, services) => {
            await handler(context, services);
            return context.ExitCode;
        };

    public static CommandHandler Create<T>(Func<InvocationContext, T, Task> handler)
        where T : notnull
        => async (context, services) => {
            await handler(context, services.GetRequiredService<T>());
            return context.ExitCode;
        };

    public static CommandHandler Create(Func<IServiceProvider, Task> handler)
        => async (context, services) => {
            await handler(services);
            return context.ExitCode;
        };

    public static CommandHandler Create<T>(Func<T, Task> handler)
        where T : notnull
        => async (context, services) => {
            await handler(services.GetRequiredService<T>());
            return context.ExitCode;
        };

    public static CommandHandler Create(Func<InvocationContext, IServiceProvider, int> handler)
        => (context, services) => new(handler(context, services));

    public static CommandHandler Create<T>(Func<InvocationContext, T, int> handler)
        where T : notnull
        => (context, services) => new(handler(context, services.GetRequiredService<T>()));

    public static CommandHandler Create(Func<IServiceProvider, int> handler)
        => (_, services) => new(handler(services));

    public static CommandHandler Create<T>(Func<T, int> handler)
        where T : notnull
        => (_, services) => new(handler(services.GetRequiredService<T>()));

    public static CommandHandler Create(Action<InvocationContext, IServiceProvider> handler)
        => (context, services) => {
            handler(context, services);
            return new(context.ExitCode);
        };

    public static CommandHandler Create<T>(Action<InvocationContext, T> handler)
        where T : notnull
        => (context, services) => {
            handler(context, services.GetRequiredService<T>());
            return new(context.ExitCode);
        };

    public static CommandHandler Create(Action<IServiceProvider> handler)
        => (context, services) => {
            handler(services);
            return new(context.ExitCode);
        };

    public static CommandHandler Create<T>(Action<T> handler)
        where T : notnull
        => (context, services) => {
            handler(services.GetRequiredService<T>());
            return new(context.ExitCode);
        };
}
