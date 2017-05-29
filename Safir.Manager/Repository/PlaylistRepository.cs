using Mehdime.Entity;

namespace Safir.Manager
{
    using Core;

    public class PlaylistRepository : DbRepository<Playlist>
    {
        public PlaylistRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator) { }
    }
}
