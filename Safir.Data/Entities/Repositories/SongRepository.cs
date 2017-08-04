// <copyright file="SongRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data.Entities.Repositories
{
    using Mehdime.Entity;

    public class SongRepository : DbRepository<Song>
    {
        public SongRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator) {
        }
    }
}
