using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cli.Services.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cli.Services
{
    internal class ConfigurationServiceDirectory : IServiceDirectory
    {
        private readonly IOptions<CliOptions> _options;
        private readonly ILogger<ConfigurationServiceDirectory> _logger;

        public ConfigurationServiceDirectory(
            IOptions<CliOptions> options,
            ILogger<ConfigurationServiceDirectory> logger)
        {
            _options = options;
            _logger = logger;
        }

        public string GetInstallationDirectory(IEnumerable<string>? extraPaths = null)
        {
            List<string> extraDirs = new();
            List<string> extraParts = new();

            if (extraPaths != null)
            {
                _logger.LogDebug("Resolving extra paths");
                var filtered = extraPaths.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                var rooted = filtered.Where(RootedAndExists).ToList();

                // If passed a valid rooted path, return the first that exists 
                if (rooted.Count >= 1) return rooted.First();

                var relativeDirs = filtered.ToLookup(x => x.Contains(Path.DirectorySeparatorChar));
                extraDirs.AddRange(relativeDirs[true]);
                extraParts.AddRange(relativeDirs[false]);
            }

            var toJoin = new[] {
                _options.Value.Config.Directory,
                ServiceOptions.DefaultInstallationDirectory,
            }.Concat(extraParts);

            return Path.Join(toJoin.ToArray());
        }

        private static bool RootedAndExists(string path) => Path.IsPathRooted(path) && Directory.Exists(path);
    }
}
