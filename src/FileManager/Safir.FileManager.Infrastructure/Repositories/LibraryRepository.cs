using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Safir.Common.Data;
using Safir.FileManager.Domain.Entities;
using Safir.FileManager.Domain.Repositories;
using Safir.FileManager.Infrastructure.Data;

namespace Safir.FileManager.Infrastructure.Repositories
{
    internal class LibraryRepository : ILibraryRepository
    {
        private readonly FileContext _context;

        public LibraryRepository(FileContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;
        
        public ValueTask<Library> GetAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Libraries.FindAsync(new object[] { id }, cancellationToken);
        }

        public IAsyncEnumerable<Library> GetAllAsync()
        {
            return _context.Libraries;
        }
    }
}
