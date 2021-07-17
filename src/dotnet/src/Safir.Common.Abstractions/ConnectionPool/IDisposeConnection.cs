using System.Threading.Tasks;

namespace Safir.Common.ConnectionPool
{
    public interface IDisposeConnection<in T>
    {
        ValueTask DisposeAsync(T connection);
    }
}
