// <copyright file="ArtistRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data.Entities.Repositories
{
    using Mehdime.Entity;

    public class ArtistRepository : DbRepository<Artist>
    {
        public ArtistRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator) {
        }
    }
}
