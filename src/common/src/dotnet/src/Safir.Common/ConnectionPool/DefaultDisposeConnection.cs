using System;
using System.Threading.Tasks;

namespace Safir.Common.ConnectionPool
{
    internal sealed class DefaultDisposeConnection<T> : IDisposeConnection<T>
    {
        public ValueTask DisposeAsync(T connection)
        {
            if (connection is IAsyncDisposable asyncDisposable)
                return asyncDisposable.DisposeAsync();
            
            if (connection is IDisposable disposable)
                disposable.Dispose();

            return new();
        }
    }
}
