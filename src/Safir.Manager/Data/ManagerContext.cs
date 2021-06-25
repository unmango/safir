using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Safir.EventSourcing.EntityFrameworkCore;
using Safir.Manager.Domain;

namespace Safir.Manager.Data
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    internal abstract class ManagerContext : EventSourcingContext
    {
        public DbSet<File> Files => Set<File>();
    }
}
