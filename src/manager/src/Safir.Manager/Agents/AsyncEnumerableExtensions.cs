using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;

namespace Safir.Manager.Agents
{
    internal static class AsyncEnumerableExtensions
    {
        public static AsyncServerStreamingCall<T> AsAsyncServerStreamingCall<T>(this IAsyncEnumerable<T> enumerable)
        {
            var reader = new AsyncEnumerableAsyncStreamReader<T>(enumerable);

            return new(
                reader,
                Task.FromResult(Metadata.Empty),
                () => Status.DefaultSuccess,
                () => Metadata.Empty,
                () => reader.DisposeAsync().AsTask().GetAwaiter().GetResult());
        }
    }
}
