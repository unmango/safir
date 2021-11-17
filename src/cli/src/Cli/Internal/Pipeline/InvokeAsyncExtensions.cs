using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Internal.Pipeline
{
    internal static class InvokeAsyncExtensions
    {
        // Alias to match the name of delegate.Invoke
        public static ValueTask Invoke<T>(
            this InvokeAsync<T> invocation,
            T context,
            CancellationToken cancellationToken = default)
            => invocation.InvokeAsync(context, cancellationToken);

        public static ValueTask InvokeAsync<T>(
            this InvokeAsync<T> invocation,
            T context,
            CancellationToken cancellationToken = default)
            => invocation.InvokeAsync(context, _ => ValueTask.CompletedTask, cancellationToken);
        
        public static ValueTask InvokeAsync<T>(
            this InvokeAsync<T> invocation,
            T context,
            Func<T, ValueTask> next,
            CancellationToken cancellationToken = default)
            => invocation(context, next, cancellationToken);
    }
}
