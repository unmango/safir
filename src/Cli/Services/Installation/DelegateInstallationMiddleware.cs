using Cli.Internal.Pipeline;

namespace Cli.Services.Installation
{
    internal class DelegateInstallationMiddleware : DelegateBehaviour<InstallationContext>, IInstallationMiddleware
    {
        public DelegateInstallationMiddleware(
            AppliesTo<InstallationContext> appliesTo,
            InvokeAsync<InstallationContext> invokeAsync)
            : base(appliesTo, invokeAsync)
        { }
    }
}
