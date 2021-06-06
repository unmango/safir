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
        long AggregateId,
        string Type,
        byte[] Data,
        DateTime Occurred,
        Metadata Metadata,
        int Version)
    {
        internal static Event Empty = new(
            default,
            nameof(Event),
            Array.Empty<byte>(),
            default,
            new Metadata(),
            default);

        // For EF binding
        private Event(long aggregateId, string type, byte[] data, DateTime occurred, int version)
            : this(aggregateId, type, data, occurred, Metadata.Empty, version) { }

        public long Id { get; init; }

        public int Position { get; init; }
    }
}
