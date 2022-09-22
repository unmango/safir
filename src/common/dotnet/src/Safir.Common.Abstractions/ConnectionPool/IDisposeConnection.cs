namespace Safir.Common.ConnectionPool;

public interface IDisposeConnection<in T>
{
    ValueTask DisposeAsync(T connection);
}
