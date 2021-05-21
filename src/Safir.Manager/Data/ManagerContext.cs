using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Safir.Manager.Data.Configuration;
using Safir.Manager.Domain;

namespace Safir.Manager.Data
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    internal abstract class ManagerContext : DbContext
    {
        public DbSet<FileCreated> FileCreated { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FileCreatedConfiguration());
        }
    }
}
