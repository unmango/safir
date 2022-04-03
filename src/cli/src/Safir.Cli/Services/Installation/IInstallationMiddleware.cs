using System;
using System.Threading;
using System.Threading.Tasks;
using Safir.Cli.Internal.Pipeline;

namespace Safir.Cli.Services.Installation;

internal delegate bool AppliesTo(InstallationContext context);
    
internal delegate ValueTask InvokeAsync(
    InstallationContext context,
    Func<InstallationContext, ValueTask> next,
    CancellationToken cancellationToken);
    
internal interface IInstallationMiddleware :
    IPipelineBehaviour<InstallationContext>,
    IAppliesTo<InstallationContext>
{
}