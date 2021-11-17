using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Safir.Common.ConnectionPool
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class ConnectionPoolOptions<T>
    {
        public const int DefaultSize = 10;
        
        internal Func<IEnumerable<T>, T>? Selector { get; set; }

        public int PoolSize { get; set; } = DefaultSize;
    }
}
