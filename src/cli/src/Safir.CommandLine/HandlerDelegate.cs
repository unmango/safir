using System.CommandLine.Parsing;

namespace Safir.CommandLine;

internal static class HandlerDelegate
{
    public static CommandHandler Create<T>(Func<T, ParseResult, CancellationToken, Task<int>> handler)
        where T : notnull
        => context => new(handler(context.GetRequiredService<T>(), context.GetParseResult(), context.GetCancellationToken()));

    public static CommandHandler Create<T>(Func<T, ParseResult, CancellationToken, Task> handler)
        where T : notnull
        => async context => {
            await handler(context.GetRequiredService<T>(), context.GetParseResult(), context.GetCancellationToken());
            return context.InvocationContext.ExitCode;
        };

    public static CommandHandler Create<T>(Func<T, ParseResult, int> handler)
        where T : notnull
        => context => new(handler(context.GetRequiredService<T>(), context.GetParseResult()));

    public static CommandHandler Create<T>(Action<T, ParseResult> handler)
        where T : notnull
        => context => {
            handler(context.GetRequiredService<T>(), context.GetParseResult());
            return new(context.InvocationContext.ExitCode);
        };
}
