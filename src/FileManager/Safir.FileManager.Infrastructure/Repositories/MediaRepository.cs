using Safir.Common.Data;
using Safir.FileManager.Domain.Repositories;
using Safir.FileManager.Infrastructure.Data;

namespace Safir.FileManager.Infrastructure.Repositories
{
    internal class MediaRepository : IMediaRepository
    {
        private readonly FileContext _context;

        public MediaRepository(FileContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;
    }
}
