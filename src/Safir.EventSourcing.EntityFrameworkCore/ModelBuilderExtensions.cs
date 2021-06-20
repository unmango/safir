using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    [PublicAPI]
    public static class ModelBuilderExtensions
    {
        public static void ApplyEventConfiguration<TEvent, TId>(this ModelBuilder builder)
            where TEvent : Event<TId>
        {
            builder.ApplyConfiguration(new EventConfiguration<TEvent, TId>());
        }
        
        public static void ApplyEventConfiguration<T>(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EventConfiguration<T>());
        }
        
        public static void ApplyEventConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EventConfiguration());
        }
    }
}
