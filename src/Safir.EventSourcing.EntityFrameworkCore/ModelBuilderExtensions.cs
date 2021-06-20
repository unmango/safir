using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    [PublicAPI]
    public static class ModelBuilderExtensions
    {
        public static void ApplyAggregateConfigurations(this ModelBuilder builder, Assembly assembly)
        {
            var aggregateTypes = assembly.DefinedTypes.Where(x =>
                !IsAggregate(x) && // Just to be sure, also testing
                x.ImplementedInterfaces.Any(IsAggregate));

            foreach (var typeInfo in aggregateTypes)
                ApplyAggregateConfiguration(builder, typeInfo);

            static bool IsAggregate(Type type) =>
                type == typeof(IAggregate<>) ||
                type == typeof(IAggregate);
        }

        public static void ApplyAggregateConfiguration(this ModelBuilder builder, Type type)
        {
            ApplyAggregateConfiguration(builder.Entity(type));
        }

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

        private static void ApplyAggregateConfiguration(EntityTypeBuilder builder)
        {
            builder.HasKey(nameof(IAggregate.Id));
            
            builder.Property(nameof(IAggregate.Id)).IsRequired();
            builder.Property(nameof(IAggregate.Version)).IsRequired();
            builder.Property(nameof(IAggregate.Events)); // TODO: Ignore?
        }
    }
}
