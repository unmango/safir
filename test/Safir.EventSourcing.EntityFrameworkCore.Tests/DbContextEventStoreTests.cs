using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Safir.Messaging;
using Xunit;

namespace Safir.EventSourcing.EntityFrameworkCore.Tests
{
    public class DbContextEventStoreTests : IAsyncDisposable
    {
        private readonly AutoMocker _mocker = new();
        private readonly TestContext _context = new();
        private readonly Mock<IEventSerializer> _serializer;
        private readonly DbContextEventStore<TestContext> _store;

        public DbContextEventStoreTests()
        {
            _mocker.Use(_context);
            _serializer = _mocker.GetMock<IEventSerializer>();
            _store = _mocker.CreateInstance<DbContextEventStore<TestContext>>();
        }

        [Fact]
        public async Task AddAsync_AddsAndSavesEvent()
        {
            var id = Guid.NewGuid();
            IEvent value = new MockEvent();
            var serialized = new Event(id, "type", Array.Empty<byte>(), DateTime.Now, new Metadata(), 69);
            _serializer.Setup(x => x.SerializeAsync(id, value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(serialized)
                .Verifiable();

            await _store.AddAsync(id, value);
            
            _serializer.Verify();
            Assert.Contains(serialized, _context.Set<Event>());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task AddAsync_Enumerable_AddsAndSavesAllEvents(int count)
        {
            var id = Guid.NewGuid();
            var events = Enumerable.Repeat(new MockEvent(), count).Cast<IEvent>();
            var serialized = new Event(id, "type", Array.Empty<byte>(), DateTime.Now, new Metadata(), 69);
            _serializer.Setup(x => x.SerializeAsync(id, It.IsAny<IEvent>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(serialized);

            await _store.AddAsync(id, events);
            
            _serializer.Verify(x => x.SerializeAsync(id, It.IsAny<IEvent>(), It.IsAny<CancellationToken>()), Times.Exactly(count));
            Assert.Contains(serialized, _context.Set<Event>());
        }

        [Fact]
        public async Task GetAsync_GetsEventMatchingId()
        {
            var serialized = new Event(Guid.NewGuid(), "type", Array.Empty<byte>(), DateTime.Now, new Metadata(), 69);
            // EF sets Metadata to `null` on AddAsync for some reason... using different objects seems to fix it
            var entry = await _context.AddAsync(serialized with { Metadata = new Metadata() });
            await _context.AddAsync(serialized with { Metadata = new Metadata() });
            await _context.SaveChangesAsync();

            await _store.GetAsync(entry.Entity.Id);
            
            _serializer.Verify(x => x.DeserializeAsync(entry.Entity, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task StreamBackwardsAsync_ReturnsEventsReversed()
        {
            var id = Guid.NewGuid();
            var serialized = new Event(id, "type", Array.Empty<byte>(), DateTime.Now, new Metadata(), 69);
            // EF sets Metadata to `null` on AddAsync for some reason... using different objects seems to fix it
            var entry1 = await _context.AddAsync(serialized with { Metadata = new Metadata() });
            var entry2 = await _context.AddAsync(serialized with { Metadata = new Metadata() });
            await _context.SaveChangesAsync();
            _serializer.Setup(x => x.DeserializeAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns<Event, CancellationToken>((@event, _) => new ValueTask<IEvent>(new MockEvent(@event.Position)));

            var stream = _store.StreamBackwardsAsync(id);
            
            Assert.NotNull(stream);
            var enumerator = stream.GetAsyncEnumerator();
            
            await enumerator.MoveNextAsync();
            Assert.NotNull(enumerator.Current);
            var first = Assert.IsType<MockEvent>(enumerator.Current);
            Assert.Equal(entry2.Entity.Position, first.Position);
            
            await enumerator.MoveNextAsync();
            Assert.NotNull(enumerator.Current);
            var second = Assert.IsType<MockEvent>(enumerator.Current);
            Assert.Equal(entry1.Entity.Position, second.Position);
        }

        [Fact]
        public async Task StreamBackwardsAsync_ReturnsCorrectAggregateStream()
        {
            var id = Guid.NewGuid();
            var serialized = new Event(id, "type", Array.Empty<byte>(), DateTime.Now, new Metadata(), 69);
            // EF sets Metadata to `null` on AddAsync for some reason... using different objects seems to fix it
            var entry1 = await _context.AddAsync(serialized with { Metadata = new Metadata() });
            await _context.AddAsync(serialized with { AggregateId = Guid.NewGuid(), Metadata = new Metadata() });
            await _context.SaveChangesAsync();
            _serializer.Setup(x => x.DeserializeAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns<Event, CancellationToken>((@event, _) => new ValueTask<IEvent>(new MockEvent(@event.Position)));

            var stream = _store.StreamBackwardsAsync(id);
            
            Assert.NotNull(stream);
            var enumerator = stream.GetAsyncEnumerator();
            
            await enumerator.MoveNextAsync();
            Assert.NotNull(enumerator.Current);
            var first = Assert.IsType<MockEvent>(enumerator.Current);
            Assert.Equal(entry1.Entity.Position, first.Position);

            Assert.False(await enumerator.MoveNextAsync());
        }

        [Fact]
        public async Task StreamBackwardsAsync_ReturnsRequestedCount()
        {
            var id = Guid.NewGuid();
            var serialized = new Event(id, "type", Array.Empty<byte>(), DateTime.Now, new Metadata(), 69);
            // EF sets Metadata to `null` on AddAsync for some reason... using different objects seems to fix it
            await _context.AddAsync(serialized with { Metadata = new Metadata() });
            var entry2 = await _context.AddAsync(serialized with { Metadata = new Metadata() });
            await _context.SaveChangesAsync();
            _serializer.Setup(x => x.DeserializeAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns<Event, CancellationToken>((@event, _) => new ValueTask<IEvent>(new MockEvent(@event.Position)));

            var stream = _store.StreamBackwardsAsync(id, 1);
            
            Assert.NotNull(stream);
            var enumerator = stream.GetAsyncEnumerator();
            
            await enumerator.MoveNextAsync();
            Assert.NotNull(enumerator.Current);
            var first = Assert.IsType<MockEvent>(enumerator.Current);
            Assert.Equal(entry2.Entity.Position, first.Position);

            Assert.False(await enumerator.MoveNextAsync());
        }

        [Fact]
        public void StreamAsync_ThrowsWhenStartIsAfterEnd()
        {
            Assert.Throws<InvalidOperationException>(() => _store.StreamAsync(Guid.NewGuid(), 69, 68));
        }

        [Fact]
        public async Task StreamAsync_ReturnsCorrectAggregateStream()
        {
            var id = Guid.NewGuid();
            var serialized = new Event(id, "type", Array.Empty<byte>(), DateTime.Now, new Metadata(), 69);
            // EF sets Metadata to `null` on AddAsync for some reason... using different objects seems to fix it
            var entry1 = await _context.AddAsync(serialized with { Metadata = new Metadata() });
            await _context.AddAsync(serialized with { AggregateId = Guid.NewGuid(), Metadata = new Metadata() });
            await _context.SaveChangesAsync();
            _serializer.Setup(x => x.DeserializeAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns<Event, CancellationToken>((@event, _) => new ValueTask<IEvent>(new MockEvent(@event.Position)));

            var stream = _store.StreamAsync(id);
            
            Assert.NotNull(stream);
            var enumerator = stream.GetAsyncEnumerator();
            
            await enumerator.MoveNextAsync();
            Assert.NotNull(enumerator.Current);
            var first = Assert.IsType<MockEvent>(enumerator.Current);
            Assert.Equal(entry1.Entity.Position, first.Position);

            Assert.False(await enumerator.MoveNextAsync());
        }

        [Fact]
        public async Task StreamAsync_ReturnsEventsStartingAtStartPosition()
        {
            var id = Guid.NewGuid();
            var serialized = new Event(id, "type", Array.Empty<byte>(), DateTime.Now, new Metadata(), 69);
            // EF sets Metadata to `null` on AddAsync for some reason... using different objects seems to fix it
            await _context.AddAsync(serialized with { Metadata = new Metadata() });
            var entry2 = await _context.AddAsync(serialized with { Metadata = new Metadata() });
            await _context.SaveChangesAsync();
            _serializer.Setup(x => x.DeserializeAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns<Event, CancellationToken>((@event, _) => new ValueTask<IEvent>(new MockEvent(@event.Position)));

            var stream = _store.StreamAsync(id, entry2.Entity.Position);
            
            Assert.NotNull(stream);
            var enumerator = stream.GetAsyncEnumerator();
            
            await enumerator.MoveNextAsync();
            Assert.NotNull(enumerator.Current);
            var first = Assert.IsType<MockEvent>(enumerator.Current);
            Assert.Equal(entry2.Entity.Position, first.Position);

            Assert.False(await enumerator.MoveNextAsync());
        }

        [Fact]
        public async Task StreamAsync_ReturnsEventsEndingAtEndPosition()
        {
            var id = Guid.NewGuid();
            var serialized = new Event(id, "type", Array.Empty<byte>(), DateTime.Now, new Metadata(), 69);
            // EF sets Metadata to `null` on AddAsync for some reason... using different objects seems to fix it
            var entry1 = await _context.AddAsync(serialized with { Metadata = new Metadata() });
            await _context.AddAsync(serialized with { Metadata = new Metadata() });
            await _context.SaveChangesAsync();
            _serializer.Setup(x => x.DeserializeAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .Returns<Event, CancellationToken>((@event, _) => new ValueTask<IEvent>(new MockEvent(@event.Position)));

            var stream = _store.StreamAsync(id, 0, entry1.Entity.Position);
            
            Assert.NotNull(stream);
            var enumerator = stream.GetAsyncEnumerator();
            
            await enumerator.MoveNextAsync();
            Assert.NotNull(enumerator.Current);
            var first = Assert.IsType<MockEvent>(enumerator.Current);
            Assert.Equal(entry1.Entity.Position, first.Position);

            Assert.False(await enumerator.MoveNextAsync());
        }

        private record MockEvent : IEvent
        {
            public MockEvent()
            {
                Position = default;
            }
            
            public MockEvent(int position)
            {
                Position = position;
            }
            
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public DateTime Occurred { get; }
            
            public int Position { get; }
        }

        public ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();
        }
    }
}
