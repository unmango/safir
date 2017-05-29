using Mehdime.Entity;

namespace Safir.Manager
{
    using Core;

    public class SongRepository : DbRepository<Song>
    {
        public SongRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator) { }
    }
}
