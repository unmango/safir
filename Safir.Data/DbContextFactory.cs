// <copyright file="DbContextFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data
{
    using System;
    using System.Data.Entity;
    using Mehdime.Entity;

    public class DbContextFactory : IDbContextFactory
    {
        private DatabaseManager _manager;

        public DbContextFactory(DatabaseManager manager) {
            _manager = manager;
        }

        public TDbContext CreateDbContext<TDbContext>()
            where TDbContext : DbContext {
            return (TDbContext)Activator.CreateInstance(
                typeof(TDbContext),
                _manager.ConnectionString);
        }
    }
}
