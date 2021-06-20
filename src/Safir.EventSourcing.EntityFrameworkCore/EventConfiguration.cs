using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    [PublicAPI]
    public class EventConfiguration : EventConfiguration<Event, Guid>
    {
        public override void Configure(EntityTypeBuilder<Event> builder)
        {
            base.Configure(builder);
            builder.HasBaseType<Event<Guid>>();
        }
    }

    [PublicAPI]
    public class EventConfiguration<T> : EventConfiguration<Event<T>, T> { }

    [PublicAPI]
    public class EventConfiguration<TEvent, TId> : IEntityTypeConfiguration<TEvent>
        where TEvent : Event<TId>
    {
        public virtual void Configure(EntityTypeBuilder<TEvent> builder)
        {
            builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.AggregateId).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Data).IsRequired();
            builder.Property(x => x.Position).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Occurred).IsRequired();
            builder.Property(x => x.Version).IsRequired();

            builder.HasKey(x => new { x.AggregateId, x.Position });

            builder.OwnsOne(x => x.Metadata, metadataBuilder => {
                metadataBuilder.Property(x => x.CorrelationId).IsRequired();
                metadataBuilder.Property(x => x.CausationId).IsRequired();
            });
        }
    }
}
