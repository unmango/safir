namespace Safir.Agent.Domain
{
    using File = System.IO.File;

    internal sealed class SystemFileWrapper : IFile
    {
        public bool Exists(string? path)
        {
            return File.Exists(path);
        }
    }
}
