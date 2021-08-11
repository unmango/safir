using System.Collections.Generic;
using System.Threading;
using Xunit;

#nullable disable

namespace Safir.Linq.Async.Tests
{
    public static class TestExtensions
    {
        public static IAsyncEnumerable<T> RunOnce<T>(this IAsyncEnumerable<T> source) =>
            source == null ? null : new RunOnceEnumerable<T>(source);

        private class RunOnceEnumerable<T> : IAsyncEnumerable<T>
        {
            private readonly IAsyncEnumerable<T> _source;
            private bool _called;

            public RunOnceEnumerable(IAsyncEnumerable<T> source)
            {
                _source = source;
            }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                Assert.False(_called);
                _called = true;
                return _source.GetAsyncEnumerator(cancellationToken);
            }
        }
    }
}
