using Microsoft.Extensions.FileProviders;

namespace Safir.Agent.Services
{
    internal sealed class PhysicalFileProviderFactory : IFileProviderFactory
    {
        public IFileProvider Create(string path)
        {
            return new PhysicalFileProvider(path);
        }
    }
}
