using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;

namespace Safir.Manager.Agents
{
    internal class AsyncEnumerableAsyncStreamReader<T> : IAsyncStreamReader<T>, IAsyncDisposable
    {
        private readonly IAsyncEnumerator<T> _enumerator;

        public AsyncEnumerableAsyncStreamReader(IAsyncEnumerable<T> enumerable)
        {
            _enumerator = enumerable.GetAsyncEnumerator();
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return _enumerator.MoveNextAsync(cancellationToken).AsTask();
        }

        public T Current => _enumerator.Current;

        public ValueTask DisposeAsync() => _enumerator.DisposeAsync();
    }
}
