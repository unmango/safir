using System;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Configuration;

internal interface ILocalConfiguration<out T>
    where T : class, new()
{
    ValueTask UpdateAsync(Action<T> update, CancellationToken cancellationToken);
}
