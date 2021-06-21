using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.Common
{
    [PublicAPI]
    public static class EnumerableExtensions
    {
        public static IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> enumerable)
            => new SynchronousAsyncEnumerable<T>(enumerable);

        private class SynchronousAsyncEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly IEnumerable<T> _enumerable;

            public SynchronousAsyncEnumerable(IEnumerable<T> enumerable)
            {
                _enumerable = enumerable;
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new Enumerator(_enumerable.GetEnumerator());
            }

            private class Enumerator : IAsyncEnumerator<T>
            {
                private readonly IEnumerator<T> _enumerator;

                public Enumerator(IEnumerator<T> enumerator)
                {
                    _enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
                }

                public ValueTask DisposeAsync()
                {
                    _enumerator.Dispose();
                    return new ValueTask();
                }

                public ValueTask<bool> MoveNextAsync()
                {
                    return new(_enumerator.MoveNext());
                }

                public T Current => _enumerator.Current;
            }
        }
    }
}
