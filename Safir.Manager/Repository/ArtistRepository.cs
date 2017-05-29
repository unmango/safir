using Mehdime.Entity;

namespace Safir.Manager
{
    using Core;

    public class ArtistRepository : DbRepository<Artist>
    {
        public ArtistRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator) { }
    }
}
