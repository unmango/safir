using System;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Common.ConnectionPool
{
    public class CreateConnection<T> : ICreateConnection<T>
    {
        private readonly Func<CancellationToken, Task<T>> _connect;

        public CreateConnection(Func<CancellationToken, Task<T>> connect)
        {
            _connect = connect;
        }
        
        public CreateConnection(Func<T> connect)
        {
            _connect = _ => Task.FromResult(connect());
        }
        
        public Task<T> ConnectAsync(CancellationToken cancellationToken = default)
        {
            return _connect(cancellationToken);
        }
    }
}
