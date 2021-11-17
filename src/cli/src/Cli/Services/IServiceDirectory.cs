using System.Collections.Generic;

namespace Cli.Services
{
    public interface IServiceDirectory
    {
        string GetInstallationDirectory(IEnumerable<string>? extraPaths = null);
    }
}
