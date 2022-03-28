using System.Collections.Generic;

namespace Safir.Cli.Services
{
    public interface IServiceDirectory
    {
        string GetInstallationDirectory(IEnumerable<string>? extraPaths = null);
    }
}
