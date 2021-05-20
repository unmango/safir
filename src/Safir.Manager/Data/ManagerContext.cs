using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Safir.Manager.Configuration;
using Safir.Manager.Domain;

namespace Safir.Manager.Data
{
    internal class ManagerContext : DbContext
    {
        private readonly IOptions<ManagerOptions> _options;

        public ManagerContext(IOptions<ManagerOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public DbSet<FileCreated> FileCreated { get; } = null!;
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var options = _options.Value;
            
            if (options.IsSelfContained)
            {
                var database = options.SqliteDatabase ?? ManagerOptions.DefaultSqliteDatabase;
                
                var builder = new SqliteConnectionStringBuilder {
                    DataSource = Path.Combine(options.DataDirectory, database)
                };

                optionsBuilder.UseSqlite(builder.ConnectionString);
            }
            else
            {
                var builder = new NpgsqlConnectionStringBuilder {
                    Database = options.PostgresDatabase ?? ManagerOptions.DefaultPostgresDatabase,
                    Host = options.PostgresHost,
                    Username = options.PostgresUsername,
                    Password = options.PostgresPassword
                };
                
                optionsBuilder.UseNpgsql(builder.ConnectionString);
            }
        }
    }
}
