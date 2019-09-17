using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Safir.Common.Data;
using Safir.FileManager.Domain.Entities;

namespace Safir.FileManager.Domain.Repositories
{
    public interface ILibraryRepository : IRepository
    {
        ValueTask<Library> GetAsync(int id, CancellationToken cancellationToken = default);

        IAsyncEnumerable<Library> GetAllAsync();
    }
}
