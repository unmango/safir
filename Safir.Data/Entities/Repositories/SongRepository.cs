using Mehdime.Entity;

namespace Safir.Data.Entities.Repositories
{
    using Core;

    public class SongRepository : DbRepository<Song>
    {
        public SongRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator) { }
    }
}
