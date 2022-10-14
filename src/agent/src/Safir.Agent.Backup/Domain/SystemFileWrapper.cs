namespace Safir.Agent.Domain;

internal sealed class SystemFileWrapper : IFile
{
    public bool Exists(string? path)
    {
        return File.Exists(path);
    }
}
