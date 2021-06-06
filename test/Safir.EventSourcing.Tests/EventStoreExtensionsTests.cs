using System.Threading;
using Moq;
using Moq.AutoMock;
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
        public void StreamAsync_PassesDefaultValues()
        {
            const long aggregateId = 420;

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
            const long aggregateId = 420;
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
            const long aggregateId = 420;

            _store.Object.StreamBackwardsAsync(aggregateId, default(CancellationToken));

            _store.Verify(x => x.StreamBackwardsAsync(aggregateId, null, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public void StreamFromAsync_PassesDefaultEndPosition()
        {
            const long aggregateId = 420;
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
            const long aggregateId = 420;
            const int endPosition = 69;

            _store.Object.StreamUntilAsync(aggregateId, endPosition);

            _store.Verify(x => x.StreamAsync(
                aggregateId,
                0,
                endPosition,
                default));
        }
    }
}
