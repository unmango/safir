using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Internal
{
    internal delegate bool AppliesTo<in T>(T context);

    internal delegate ValueTask InvokeAsync<T>(
        T context,
        Func<T, ValueTask> next,
        CancellationToken cancellationToken = default);
    
    internal interface IPipelineBehaviour<T> where T : class
    {
        bool AppliesTo(T context);
        
        ValueTask InvokeAsync(T context, Func<T, ValueTask> next, CancellationToken cancellationToken = default);
    }
}
