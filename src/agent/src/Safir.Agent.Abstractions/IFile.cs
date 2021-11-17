using JetBrains.Annotations;

namespace Safir.Agent
{
    [PublicAPI]
    public interface IFile
    {
        bool Exists(string? path);
    }
}
