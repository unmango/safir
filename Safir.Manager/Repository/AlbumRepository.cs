using Mehdime.Entity;

namespace Safir.Manager
{
    using Core;

    public class AlbumRepository : DbRepository<Album>
    {
        public AlbumRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator) { }
    }
}
