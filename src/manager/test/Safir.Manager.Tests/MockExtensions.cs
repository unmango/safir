using Grpc.Core;
using Moq.Language.Flow;

namespace Safir.Manager.Tests;

internal static class MockExtensions
{
    public static IReturnsResult<TClient> ReturnsAsync<TClient, TItem>(
        this ISetup<TClient, AsyncServerStreamingCall<TItem>> setup,
        IEnumerable<TItem> result)
        where TClient : class
        => setup.ReturnsAsync(result.ToAsyncEnumerable());

    public static IReturnsResult<TClient> ReturnsAsync<TClient, TItem>(
        this ISetup<TClient, AsyncServerStreamingCall<TItem>> setup,
        IAsyncEnumerable<TItem> result)
        where TClient : class
    {
        var enumerator = result.GetAsyncEnumerator();
        return setup.Returns(new AsyncServerStreamingCall<TItem>(
            new AsyncEnumerableStreamReader<TItem>(enumerator),
            Task.FromResult(Metadata.Empty),
            () => Status.DefaultSuccess,
            () => Metadata.Empty,
            () => { }));
    }

    private sealed class AsyncEnumerableStreamReader<T> : IAsyncStreamReader<T>
    {
        private readonly IAsyncEnumerator<T> _enumerator;

        public AsyncEnumerableStreamReader(IAsyncEnumerator<T> enumerator) => _enumerator = enumerator;

        public Task<bool> MoveNext(CancellationToken cancellationToken)
            => _enumerator.MoveNextAsync(cancellationToken).AsTask();

        public T Current => _enumerator.Current;
    }
}
