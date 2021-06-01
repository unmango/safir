using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Safir.EventSourcing.Tests
{
    public class DefaultGenericAggregateStoreTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IAggregateStore> _nonGenericStore;
        private readonly DefaultAggregateStore<FakeAggregate> _store;

        public DefaultGenericAggregateStoreTests()
        {
            _nonGenericStore = _mocker.GetMock<IAggregateStore>();
            _store = _mocker.CreateInstance<DefaultAggregateStore<FakeAggregate>>();
        }

        [Fact]
        public async Task StoreAsync_DelegateCallToNonGenericStore()
        {
            var aggregate = new FakeAggregate();

            await _store.StoreAsync(aggregate);
            
            _nonGenericStore.Verify(x => x.StoreAsync(aggregate, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GetAsync_DelegateCallToNonGenericStore()
        {
            const long id = 420;

            await _store.GetAsync(id);
            
            _nonGenericStore.Verify(x => x.GetAsync<FakeAggregate>(id, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GetAsyncWithVersion_DelegateCallToNonGenericStore()
        {
            const long id = 420;
            const int version = 69;

            await _store.GetAsync(id, version);
            
            _nonGenericStore.Verify(x => x.GetAsync<FakeAggregate>(id, version, It.IsAny<CancellationToken>()));
        }

        public record FakeAggregate : Aggregate;
    }
}
