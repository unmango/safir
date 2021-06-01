using System;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public sealed record Metadata(Guid CorrelationId, Guid CausationId)
    {
        public Metadata() : this(Guid.Empty, Guid.Empty) { }

        public Metadata(Guid correlationId) : this(correlationId, Guid.Empty) { }
    }

    [PublicAPI]
    public record Event(
        long AggregateId,
        string Type,
        ReadOnlyMemory<byte> Data,
        DateTime Occurred,
        Metadata Metadata,
        int Version)
    {
        public long Id { get; init; }

        public int Position { get; init; }

        internal static Event Empty = new(
            default,
            nameof(Event),
            ReadOnlyMemory<byte>.Empty,
            default,
            new(),
            default);
    }
}
