using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Safir.Messaging;
using Xunit;

namespace Safir.EventSourcing.Tests
{
    public class EventStoreExtensionsTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventStore> _store;

        public EventStoreExtensionsTests()
        {
            _store = _mocker.GetMock<IEventStore>();
        }

        [Fact]
        public async Task CreatAsync_CreatesNewAggregate()
        {
            var @event = new MockEvent();

            var aggregate = await _store.Object.CreateAsync<MockAggregate, Guid>(@event);
            
            _store.Verify(x => x.AddAsync(aggregate.Id, @event, It.IsAny<CancellationToken>()));
            Assert.Contains(@event, aggregate.Events);
        }

        [Fact]
        public async Task CreatAsync_CreatesNewAggregateWithAllEvents()
        {
            var events = new[] { new MockEvent(), new MockEvent() };

            var aggregate = await _store.Object.CreateAsync<MockAggregate, Guid>(events);
            
            _store.Verify(x => x.AddAsync(aggregate.Id, events, It.IsAny<CancellationToken>()));
            Assert.Equal(events, aggregate.Events);
        }

        [Fact]
        public async Task NewAsync_CreatesNewEventStream()
        {
            var @event = new MockEvent();

            var id = await _store.Object.NewAsync(@event);
            
            _store.Verify(x => x.AddAsync(id, @event, It.IsAny<CancellationToken>()));
            Assert.NotEqual(Guid.Empty, id);
        }

        [Fact]
        public async Task NewAsync_CreatesNewEventStreamWithAllEvents()
        {
            var events = new[] { new MockEvent(), new MockEvent() };

            var id = await _store.Object.NewAsync(events);
            
            _store.Verify(x => x.AddAsync(id, events, It.IsAny<CancellationToken>()));
            Assert.NotEqual(Guid.Empty, id);
        }

        [Fact]
        public void StreamAsync_PassesDefaultValues()
        {
            var aggregateId = Guid.NewGuid();

            _store.Object.StreamAsync(aggregateId);

            _store.Verify(x => x.StreamAsync(
                aggregateId,
                0,
                int.MaxValue,
                default));
        }

        [Fact]
        public void StreamAsync_PassesDefaultEndPosition()
        {
            var aggregateId = Guid.NewGuid();
            const int startPosition = 69;

            _store.Object.StreamAsync(aggregateId, startPosition);

            _store.Verify(x => x.StreamAsync(
                aggregateId,
                startPosition,
                int.MaxValue,
                default));
        }

        [Fact]
        public void StreamBackwardsAsync_PassesNullCount()
        {
            var aggregateId = Guid.NewGuid();

            _store.Object.StreamBackwardsAsync(aggregateId, default(CancellationToken));

            _store.Verify(x => x.StreamBackwardsAsync(aggregateId, null, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void StreamFromAsync_PassesDefaultEndPosition()
        {
            var aggregateId = Guid.NewGuid();
            const int startPosition = 69;

            _store.Object.StreamFromAsync(aggregateId, startPosition);

            _store.Verify(x => x.StreamAsync(
                aggregateId,
                startPosition,
                int.MaxValue,
                default));
        }

        [Fact]
        public void StreamUntilAsync_PassesDefaultStartPosition()
        {
            var aggregateId = Guid.NewGuid();
            const int endPosition = 69;

            _store.Object.StreamUntilAsync(aggregateId, endPosition);

            _store.Verify(x => x.StreamAsync(
                aggregateId,
                0,
                endPosition,
                default));
        }

        private record MockEvent : IEvent
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public DateTime Occurred { get; }
        }

        private record MockAggregate : Aggregate
        {
            protected override void Apply(IEvent @event)
            {
                Enqueue(@event);
            }
        }
    }
}
