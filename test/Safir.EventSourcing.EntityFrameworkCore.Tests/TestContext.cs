using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Safir.EventSourcing.EntityFrameworkCore.Tests
{
    public sealed class TestContext : DbContext
    {
        private readonly DbConnection _connection;

        public TestContext() : base(BuildOptions())
        {
            Database.EnsureCreated();
            _connection = Database.GetDbConnection();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventConfiguration());

            // Limitation of SQLite in memory provider
            modelBuilder.Entity<Event>().Property(x => x.Id)
                .HasValueGenerator((_, _) => new GuidValueGenerator());
            modelBuilder.Entity<Event>().Property(x => x.Position)
                .HasValueGenerator((_, _) => new SequentialIntValueGenerator());
        }

        public override void Dispose()
        {
            _connection.Dispose();
            base.Dispose();
        }

        private static DbContextOptions<TestContext> BuildOptions()
            => new DbContextOptionsBuilder<TestContext>()
                .UseSqlite(CreateDatabase())
                .Options;

        private static DbConnection CreateDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

        private class SequentialIntValueGenerator : ValueGenerator<int>
        {
            private int _last;

            public override int Next(EntityEntry entry)
            {
                return ++_last;
            }

            public override bool GeneratesTemporaryValues => false;
        }
    }
}
