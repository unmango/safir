using System;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Common.ConnectionPool
{
    internal sealed class DefaultConnectionManager<T> : IConnectionManager<T>
    {
        private readonly ICreateConnection<T> _createConnection;
        private readonly IDisposeConnection<T> _disposeConnection;

        public DefaultConnectionManager(ICreateConnection<T> createConnection, IDisposeConnection<T> disposeConnection)
        {
            _createConnection = createConnection ?? throw new ArgumentNullException(nameof(createConnection));
            _disposeConnection = disposeConnection ?? throw new ArgumentNullException(nameof(disposeConnection));
        }
        
        public Task<T> ConnectAsync(CancellationToken cancellationToken = default)
        {
            return _createConnection.ConnectAsync(cancellationToken);
        }

        public ValueTask DisposeAsync(T connection)
        {
            return _disposeConnection.DisposeAsync(connection);
        }
    }
}
