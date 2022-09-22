using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Safir.Common.Tasks;

[PublicAPI]
public static class TaskEnumerableExtensions
{
    public static TaskAwaiter<T[]> GetAwaiter<T>(this IEnumerable<Task<T>> enumerable)
    {
        return Task.WhenAll(enumerable).GetAwaiter();
    }

    public static TaskAwaiter<T[]> GetAwaiter<T>(this IEnumerable<ValueTask<T>> enumerable)
    {
        return Task.WhenAll(enumerable.Select(x => x.AsTask())).GetAwaiter();
    }
}
