using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Safir.EventSourcing.Tests
{
    public class SnapshotStoreExtensionsTests
    {
        private readonly Mock<ISnapshotStore> _store = new();

        [Fact]
        public async Task FindAsync_FindsWithDefaultMaxVersion()
        {
            var id = Guid.NewGuid();

            await _store.Object.FindAsync<TestAggregate, Guid>(id, CancellationToken.None);
            
            _store.Verify(x => x.FindAsync<TestAggregate, Guid>(id, int.MaxValue, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task FindAsync_GuidId_FindsWithSameArguments()
        {
            var id = Guid.NewGuid();

            await _store.Object.FindAsync<TestAggregate>(id);
            
            _store.Verify(x => x.FindAsync<TestAggregate, Guid>(id, It.IsAny<int>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task FindAsync_GuidId_DefaultVersion_FindsWithSameArguments()
        {
            var id = Guid.NewGuid();

            await _store.Object.FindAsync<TestAggregate>(id, CancellationToken.None);
            
            _store.Verify(x => x.FindAsync<TestAggregate, Guid>(id, int.MaxValue, It.IsAny<CancellationToken>()));
        }

        private record TestAggregate : Aggregate;
    }
}
