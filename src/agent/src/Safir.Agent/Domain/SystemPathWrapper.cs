using System.IO;

namespace Safir.Agent.Domain
{
    internal sealed class SystemPathWrapper : IPath
    {
        public string GetRelativePath(string relativeTo, string path)
        {
            return Path.GetRelativePath(relativeTo, path);
        }
    }
}
