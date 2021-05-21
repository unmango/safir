using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using Safir.Manager.Configuration;
using Safir.Manager.Data.Configuration.Postgres;

namespace Safir.Manager.Data
{
    internal class PostgresManagerContext : ManagerContext
    {
        private readonly IOptions<ManagerOptions> _options;

        public PostgresManagerContext(IOptions<ManagerOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var options = _options.Value;

            var builder = new NpgsqlConnectionStringBuilder {
                Database = options.PostgresDatabase ?? ManagerOptions.DefaultPostgresDatabase,
                Host = options.PostgresHost,
                Username = options.PostgresUsername,
                Password = options.PostgresPassword
            };

            optionsBuilder.UseNpgsql(builder.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FileCreatedConfiguration());
        }
    }
}
