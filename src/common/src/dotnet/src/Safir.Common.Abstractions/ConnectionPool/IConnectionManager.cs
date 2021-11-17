namespace Safir.Common.ConnectionPool
{
    public interface IConnectionManager<T> :
        ICreateConnection<T>,
        IDisposeConnection<T>
    { }
}
