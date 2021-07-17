using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.EventSourcing.InMemory;
using Safir.Messaging;
using Xunit;

namespace Safir.EventSourcing.Tests
{
    public class InMemoryEventStoreTests
    {
        private readonly ConcurrentDictionary<Guid, IEvent> _events = new();
        private readonly ConcurrentDictionary<Guid, object> _aggregateMap = new();
        private readonly InMemoryEventStore _store;

        public InMemoryEventStoreTests()
        {
            _store = new InMemoryEventStore(_events, _aggregateMap);
        }

        [Fact]
        public async Task AddAsync_AddsEvent()
        {
            var @event = new MockEvent();
            var id = Guid.NewGuid();

            await _store.AddAsync(id, @event);

            Assert.Contains(id, _aggregateMap.Values);
            var eventId = _aggregateMap.First(x => id.Equals(x.Value)).Key;
            var result = Assert.Contains(eventId, (IDictionary<Guid, IEvent>)_events);
            Assert.Same(@event, result);
        }

        [Fact]
        public async Task AddAsync_ThrowsWhenIdIsNull()
        {
            var @event = new MockEvent();
            object? id = null;

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => _store.AddAsync(id, @event));
        }

        [Fact]
        public async Task AddAsync_Enumerable_AddsAllEvents()
        {
            var event1 = new MockEvent();
            var event2 = new MockEvent();
            var event3 = new MockEvent();
            var id = Guid.NewGuid();

            await _store.AddAsync(id, new[] { event1, event2, event3 });
            
            Assert.Contains(id, _aggregateMap.Values);
            var eventIds = _aggregateMap.Where(x => id.Equals(x.Value)).Select(x => x.Key);
            foreach (var eventId in eventIds)
            {
                // TODO: This is jank
                var result = Assert.Contains(eventId, (IDictionary<Guid, IEvent>)_events);
                Assert.Contains(result, new[] { event1, event2, event3 });
            }
        }

        [Fact]
        public async Task GetAsync_GetsEvent()
        {
            var @event = new MockEvent();
            var id = Guid.NewGuid();
            _events.TryAdd(id, @event);

            var result = await _store.GetAsync<Guid>(id);
            
            Assert.Same(@event, result);
        }

        [Fact]
        public async Task GetAsync_ThrowsWhenIdDoesNotExist()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _store.GetAsync<Guid>(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetAsync_Generic_GetsDeserializedEvent()
        {
            var @event = new MockEvent();
            var id = Guid.NewGuid();
            _events.TryAdd(id, @event);

            var result = await _store.GetAsync<MockEvent, Guid>(id);
            
            Assert.Same(@event, result);
        }

        [Fact]
        public async Task GetAsync_Generic_ThrowsWhenIdDoesNotExist()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _store.GetAsync<MockEvent, Guid>(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetAsync_Generic_ThrowsWhenTypeIsInvalid()
        {
            var @event = new MockEvent();
            var id = Guid.NewGuid();
            _events.TryAdd(id, @event);
            
            await Assert.ThrowsAsync<InvalidCastException>(
                () => _store.GetAsync<BadEvent, Guid>(id));
        }

        [Fact]
        public async Task StreamAsync_StreamsAllEventsWithDefaultValues()
        {
            var event1 = new MockEvent(1);
            var event2 = new MockEvent(2);
            var event3 = new MockEvent(3);
            var id = Guid.NewGuid();
            var eventId1 = Guid.NewGuid();
            var eventId2 = Guid.NewGuid();
            var eventId3 = Guid.NewGuid();
            _events.TryAdd(eventId1, event1);
            _events.TryAdd(eventId2, event2);
            _events.TryAdd(eventId3, event3);
            _aggregateMap.TryAdd(eventId1, id);
            _aggregateMap.TryAdd(eventId2, id);
            _aggregateMap.TryAdd(eventId3, id);
            var events = new[] { event1, event2, event3 };

            var index = 0;
            var flag = false;
            await foreach (var @event in _store.StreamAsync(id))
            {
                flag = true;
                var mockEvent = Assert.IsType<MockEvent>(@event);
                var expected = events[index++];
                Assert.Equal(expected.Id, mockEvent.Id);
            }
            
            Assert.True(flag, "No events were streamed");
        }

        private record MockEvent : IEvent
        {
            public MockEvent() { }

            public MockEvent(int id)
            {
                Id = id;
            }
            
            public int Id { get; }
            
            public DateTime Occurred { get; } = DateTime.Now;
        }

        [UsedImplicitly]
        private record BadEvent : IEvent
        {
            public DateTime Occurred { get; } = DateTime.Now;
        }
    }
}
