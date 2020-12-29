using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using SimpleExec;

namespace Cli.Services.Sources
{
    internal class GitServiceSource : IServiceSource
    {
        public ValueTask<CanInitializeResult> CanInitializeAsync(CancellationToken cancellationToken = default)
        {
            var path = Environment.GetEnvironmentVariable("PATH");

            var result = CanInitializeResult.Success;

            if (!path?.Contains("git", StringComparison.OrdinalIgnoreCase) ?? false)
                result = CanInitializeResult.Fail("`git` was not found on PATH");

            return new ValueTask<CanInitializeResult>(result);
        }

        public async Task InitializeAsync(
            ServiceEntry service,
            string workingDirectory,
            CancellationToken cancellationToken = default)
        {
            if (!Satisfies(service))
                throw new NotSupportedException($"{GetType()} cannot initialize service {service.Name}");

            var args = string.Join(' ', "clone", service.GitCloneUrl);
            await Command.RunAsync("git", args, workingDirectory, cancellationToken: cancellationToken);

            throw new System.NotImplementedException();
        }

        public ValueTask<bool> IsInitializedAsync(
            ServiceEntry service,
            string workingDirectory,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public bool Satisfies(ServiceEntry service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            return service.Source == ServiceSource.Git && !string.IsNullOrWhiteSpace(service.GitCloneUrl);
        }
    }
}
