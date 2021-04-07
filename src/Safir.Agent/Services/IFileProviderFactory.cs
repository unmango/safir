using Microsoft.Extensions.FileProviders;

namespace Safir.Agent.Services
{
    public interface IFileProviderFactory
    {
        IFileProvider Create(string path);
    }
}
