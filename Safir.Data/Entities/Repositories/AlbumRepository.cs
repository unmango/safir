using Mehdime.Entity;

namespace Safir.Data.Entities.Repositories
{
    using Core;

    public class AlbumRepository : DbRepository<Album>
    {
        public AlbumRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator) { }
    }
}
