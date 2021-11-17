using System;
using System.Threading;
using System.Threading.Tasks;
using Cli.Internal.Pipeline;

namespace Cli.Services.Installation
{
    internal class DelegateInstallationMiddleware : IInstallationMiddleware
    {
        private readonly AppliesTo<InstallationContext> _appliesTo;
        private readonly InvokeAsync<InstallationContext> _invokeAsync;

        public DelegateInstallationMiddleware(
            AppliesTo<InstallationContext> appliesTo,
            InvokeAsync<InstallationContext> invokeAsync)
        {
            _appliesTo = appliesTo ?? throw new ArgumentNullException(nameof(appliesTo));
            _invokeAsync = invokeAsync ?? throw new ArgumentNullException(nameof(invokeAsync));
        }

        public bool AppliesTo(InstallationContext context) => _appliesTo(context);

        public ValueTask InvokeAsync(
            InstallationContext context,
            Func<InstallationContext, ValueTask> next,
            CancellationToken cancellationToken = default)
            => _invokeAsync(context, next, cancellationToken);
    }
}
