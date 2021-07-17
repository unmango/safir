using System;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Common.ConnectionPool
{
    public interface IConnectionPool<T> : IDisposable
    {
        ValueTask<T> GetConnectionAsync(CancellationToken cancellationToken = default);
    }
}
