using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cli.Internal;
using Cli.Internal.Progress;
using Cli.Internal.Wrappers.Git;
using Cli.Services.Configuration;
using Cli.Services.Configuration.Validation;
using Cli.Services.Sources;
using LibGit2Sharp;

namespace Cli.Services.Installation.Installers
{
    internal class GitInstaller : ServiceInstallerMiddleware
    {
        private readonly CloneOptions _options = new();
        private readonly string? _cloneUrl;
        private readonly IRepositoryFunctions _repository;
        private readonly IProgressReporter _progress;

        // ReSharper disable once MemberCanBePrivate.Global
        public GitInstaller(IRepositoryFunctions repository, IProgressReporter progress)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _progress = progress;
        }

        internal GitInstaller(string cloneUrl, IRepositoryFunctions repository, IProgressReporter progress)
            : this(repository, progress)
        {
            _cloneUrl = ValidateUrl(cloneUrl);
        }

        public override bool AppliesTo(InstallationContext context)
        {
            return context.Sources.Any(x => x is GitSource);
        }

        public override ValueTask InstallAsync(
            InstallationContext context,
            CancellationToken cancellationToken = default)
        {
            Install(context);

            return ValueTask.CompletedTask;
        }

        private void Install(InstallationContext context)
        {
            var (workingDirectory, service, sources) = context;
            var cloneDirectory = !string.IsNullOrWhiteSpace(service.Name)
                ? Path.Combine(workingDirectory, service.Name.ToLower())
                : workingDirectory;

            if (!string.IsNullOrWhiteSpace(_cloneUrl))
            {
                Clone(_cloneUrl, cloneDirectory);
                return;
            }

            foreach (var source in sources.OfType<GitSource>())
            {
                Clone(source.CloneUrl, cloneDirectory);
            }
        }

        private void Clone(string cloneUrl, string directory)
        {
            if (_repository.IsValid(directory)) return;

            _options.OnProgress = OnProgress;
            
            _repository.Clone(cloneUrl, directory, _options);
        }

        private bool OnProgress(string text)
        {
            _progress.Report(text);
            return true;
        }

        private static string ValidateUrl(string? cloneUrl)
        {
            // TODO: Add validation to GitSource and use that instead
            var result = new ServiceSource {
                Type = SourceType.Git,
                CloneUrl = cloneUrl
            }.ValidateGit();

            if (!result.IsValid)
            {
                throw new ArgumentException(result.ToString(), nameof(cloneUrl));
            }

            return cloneUrl!;
        }
    }
}
