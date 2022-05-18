using System;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Configuration;

internal interface ILocalConfiguration
{
    ValueTask UpdateAsync(Action<LocalConfiguration> update, CancellationToken cancellationToken);
}
