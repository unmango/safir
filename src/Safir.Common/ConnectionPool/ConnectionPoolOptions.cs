using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Common.ConnectionPool
{
    public sealed class ConnectionPoolOptions<T>
    {
        public const int DefaultSize = 10;
        
        internal Func<IEnumerable<T>, T>? Selector { get; set; }

        public int PoolSize { get; set; } = DefaultSize;
    }
}
