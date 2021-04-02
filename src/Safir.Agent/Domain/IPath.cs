namespace Safir.Agent.Domain
{
    public interface IPath
    {
        string GetRelativePath(string relativeTo, string path);
    }
}
