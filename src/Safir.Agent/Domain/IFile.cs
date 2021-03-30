namespace Safir.Agent.Domain
{
    public interface IFile
    {
        bool Exists(string? path);
    }
}
