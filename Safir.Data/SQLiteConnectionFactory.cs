// <copyright file="SQLiteConnectionFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Data
{
    using System.Data.Common;
    using System.Data.Entity.Infrastructure;
    using System.Data.SQLite;

    public class SQLiteConnectionFactory : IDbConnectionFactory
    {
        public DbConnection CreateConnection(string nameOrConnectionString) {
            return new SQLiteConnection(nameOrConnectionString);
        }
    }
}
