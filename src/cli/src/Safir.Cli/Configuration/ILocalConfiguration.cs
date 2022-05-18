using System;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Configuration;

internal interface ILocalConfiguration
{
    ValueTask UpdateAsync(Func<LocalConfiguration, LocalConfiguration> update, CancellationToken cancellationToken);
}
