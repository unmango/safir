using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Internal.Pipeline
{
    internal delegate ValueTask InvokeAsync<T>(
        T context,
        Func<T, ValueTask> next,
        CancellationToken cancellationToken = default);
    
    internal interface IPipelineBehaviour<T> where T : class
    {
        ValueTask InvokeAsync(T context, Func<T, ValueTask> next, CancellationToken cancellationToken = default);
    }
}
