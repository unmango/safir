using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Safir.Manager.Configuration;

namespace Safir.Manager.Data
{
    internal class ManagerContext : DbContext
    {
        private readonly IOptions<ManagerOptions> _options;

        public ManagerContext(IOptions<ManagerOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new SqliteConnectionStringBuilder {
                { "Data Source", Path.Combine(_options.Value.DataDirectory, "manager.db") }
            };

            optionsBuilder.UseSqlite(builder.ConnectionString);
        }
    }
}
