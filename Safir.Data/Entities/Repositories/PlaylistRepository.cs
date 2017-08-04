// <copyright file="PlaylistRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data.Entities.Repositories
{
    using Mehdime.Entity;

    public class PlaylistRepository : DbRepository<Playlist>
    {
        public PlaylistRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator) {
        }
    }
}
