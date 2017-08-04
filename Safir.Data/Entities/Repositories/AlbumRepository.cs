// <copyright file="AlbumRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data.Entities.Repositories
{
    using Mehdime.Entity;

    public class AlbumRepository : DbRepository<Album>
    {
        public AlbumRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator) {
        }
    }
}
