using Mehdime.Entity;

namespace Safir.Data.Entities.Repositories
{
    using Core;

    public class ArtistRepository : DbRepository<Artist>
    {
        public ArtistRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator) { }
    }
}
