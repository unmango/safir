using System;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
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
        int Version) : Event<Guid, Guid>(AggregateId, Type, Data, Occurred, Metadata, Version)
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
        int Version) : Event<T, Guid>(AggregateId, Type, Data, Occurred, Metadata, Version)
    {
        // For EF binding
        private Event(T aggregateId, string type, byte[] data, DateTime occurred, int version)
            : this(aggregateId, type, data, occurred, Metadata.Empty, version) { }
    }

    [PublicAPI]
    public record Event<TAggregateId, TId>(
        TAggregateId AggregateId,
        string Type,
        byte[] Data,
        DateTime Occurred,
        Metadata Metadata,
        int Version)
    {
        internal static Event<TAggregateId, TId> Empty = new(
            default!, // TODO: Nullability
            nameof(Event<TAggregateId, TId>),
            Array.Empty<byte>(),
            default,
            new Metadata(),
            default);

        // For EF binding
        private Event(TAggregateId aggregateId, string type, byte[] data, DateTime occurred, int version)
            : this(aggregateId, type, data, occurred, Metadata.Empty, version) { }

        public TId Id { get; init; } = default!; // TODO: Nullability

        public int Position { get; init; }
    }
}
