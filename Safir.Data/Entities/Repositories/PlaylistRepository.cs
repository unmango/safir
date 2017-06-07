using Mehdime.Entity;

namespace Safir.Data.Entities.Repositories
{
    using Core;

    public class PlaylistRepository : DbRepository<Playlist>
    {
        public PlaylistRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator) { }
    }
}
