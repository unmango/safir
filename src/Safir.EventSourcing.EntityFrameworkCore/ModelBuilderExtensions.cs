using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ModelBuilderExtensions
    {
        public static void ConfigureEvents(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EventConfiguration());
            builder.ApplyConfiguration(new MetadataConfiguration());
        }
    }
}
