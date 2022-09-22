namespace Safir.Common.ConnectionPool;

public interface ICreateConnection<T>
{
    Task<T> ConnectAsync(CancellationToken cancellationToken = default);
}
