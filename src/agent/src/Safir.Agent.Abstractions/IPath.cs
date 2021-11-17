using JetBrains.Annotations;

namespace Safir.Agent
{
    [PublicAPI]
    public interface IPath
    {
        string GetRelativePath(string relativeTo, string path);
    }
}
