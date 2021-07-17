using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.Common.Tasks
{
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
}
