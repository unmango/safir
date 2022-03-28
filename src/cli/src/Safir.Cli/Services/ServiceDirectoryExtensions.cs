using System.Linq;

namespace Safir.Cli.Services
{
    internal static class ServiceDirectoryExtensions
    {
        public static string GetInstallationDirectory(
            this IServiceDirectory serviceDirectory,
            params string?[] extraPaths)
            => serviceDirectory.GetInstallationDirectory(
                // TODO: WTF there has to be a better way for this
                extraPaths.Where(x => x != null).Select(x => x!));
    }
}
