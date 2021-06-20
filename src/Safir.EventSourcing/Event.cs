using System;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    // TODO: Consider optional properties
    [PublicAPI]
    public sealed record Metadata(Guid CorrelationId, Guid CausationId)
    {
        public static Metadata Empty = new();

        public Metadata() : this(Guid.Empty, Guid.Empty) { }

        public Metadata(Guid correlationId) : this(correlationId, Guid.Empty) { }
    }

    [PublicAPI]
    public record Event(
        Guid AggregateId,
        string Type,
        byte[] Data,
        DateTime Occurred,
        Metadata Metadata,
        int Version) : Event<Guid>(AggregateId, Type, Data, Occurred, Metadata, Version)
    {
        // For EF binding
        private Event(Guid aggregateId, string type, byte[] data, DateTime occurred, int version)
            : this(aggregateId, type, data, occurred, Metadata.Empty, version) { }
    }

    [PublicAPI]
    public record Event<T>(
        T AggregateId,
        string Type,
        byte[] Data,
        DateTime Occurred,
        Metadata Metadata,
        int Version)
    {
        internal static Event<T> Empty = new(
            default!, // TODO: Nullability
            nameof(Event<T>),
            Array.Empty<byte>(),
            default,
            new Metadata(),
            default);

        // For EF binding
        private Event(T aggregateId, string type, byte[] data, DateTime occurred, int version)
            : this(aggregateId, type, data, occurred, Metadata.Empty, version) { }

        public Guid Id { get; init; }

        public int Position { get; init; }
    }
}
