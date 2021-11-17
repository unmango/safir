using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Safir.Messaging;
using Xunit;

namespace Safir.EventSourcing.Tests
{
    public class DefaultAggregateStoreTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventStore> _eventStore;
        private readonly DefaultAggregateStore _store;

        public DefaultAggregateStoreTests()
        {
            _eventStore = _mocker.GetMock<IEventStore>();
            _store = _mocker.CreateInstance<DefaultAggregateStore>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task StoreAsync_AddsAllEvents(int count)
        {
            var events = Enumerable.Repeat(new FakeEvent(), count).Cast<IEvent>().ToArray();
            var aggregate = new FakeAggregate(events);

            await _store.StoreAsync(aggregate);

            _eventStore.Verify(x => x.AddAsync(aggregate.Id, It.Is<IEnumerable<IEvent>>(y => y.Count() == count),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task StoreAsync_DoesntPersistWhenNoEvents()
        {
            var aggregate = new FakeAggregate();

            await _store.StoreAsync(aggregate);

            _eventStore.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task GetAsync_GetsAggregate(int count)
        {
            var events = AsyncEnumerable.Repeat(new FakeEvent(), count);
            var id = Guid.NewGuid();
            _eventStore
                .Setup(x => x.StreamAsync(id, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(events);

            var aggregate = await _store.GetAsync<FakeAggregate>(id);

            _eventStore.Verify();
            Assert.NotNull(aggregate);
            Assert.Equal(count, aggregate.NumApplied);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task GetAsync_GetsAggregateWithVersion(int count)
        {
            var events = AsyncEnumerable.Repeat(new FakeEvent(), count);
            var id = Guid.NewGuid();
            const int version = 69;
            _eventStore.Setup(x => x.StreamAsync(id, version, It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(events);

            var aggregate = await _store.GetAsync<FakeAggregate>(id, version);

            _eventStore.Verify();
            Assert.NotNull(aggregate);
            Assert.Equal(count, aggregate.NumApplied);
        }

        private record FakeEvent : IEvent
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public DateTime Occurred { get; }
        }

        private record FakeAggregate : Aggregate
        {
            public FakeAggregate() { }

            public FakeAggregate(params IEvent[] events)
            {
                foreach (var @event in events)
                {
                    Enqueue(@event);
                }
            }

            public int NumApplied;

            protected override void Apply(IEvent @event)
            {
                NumApplied++;
            }
        }
    }
}
