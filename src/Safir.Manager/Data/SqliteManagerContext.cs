using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Safir.EventSourcing.EntityFrameworkCore;
using Safir.Manager.Configuration;

namespace Safir.Manager.Data
{
    internal class SqliteManagerContext : ManagerContext
    {
        private readonly IOptions<ManagerOptions> _options;

        public SqliteManagerContext(IOptions<ManagerOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var options = _options.Value;
            var database = options.SqliteDatabase ?? ManagerOptions.DefaultSqliteDatabase;
                
            var builder = new SqliteConnectionStringBuilder {
                DataSource = Path.Combine(options.DataDirectory, database)
            };

            optionsBuilder.UseSqlite(builder.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyEventConfiguration();
        }
    }
}
