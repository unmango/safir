namespace Safir.Common.ConnectionPool;

public interface IConnectionPool<T> : IDisposable
{
    ValueTask<T> GetConnectionAsync(CancellationToken cancellationToken = default);
}
