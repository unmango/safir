using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    [PublicAPI]
    public static class ModelBuilderExtensions
    {
        public static void ApplyEventConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EventConfiguration());
        }
    }
}
