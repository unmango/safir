using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Safir.Manager.Domain;

namespace Safir.Manager.Data
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    internal abstract class ManagerContext : DbContext
    {
        public DbSet<Event<string>> FileEvents { get; set; } = null!;

        public DbSet<File> Files { get; set; } = null!;
    }
}
