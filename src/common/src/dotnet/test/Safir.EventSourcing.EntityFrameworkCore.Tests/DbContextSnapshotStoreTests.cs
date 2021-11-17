using System;
using System.Threading.Tasks;
using Moq.AutoMock;
using Xunit;

namespace Safir.EventSourcing.EntityFrameworkCore.Tests
{
    // TODO: Test everything
    public class DbContextSnapshotStoreTests : IAsyncDisposable
    {
        private readonly AutoMocker _mocker = new();
        private readonly TestContext _context = new();
        private readonly DbContextSnapshotStore<TestContext> _store;

        public DbContextSnapshotStoreTests()
        {
            _mocker.Use(_context);
            _store = _mocker.CreateInstance<DbContextSnapshotStore<TestContext>>();
        }

        // [Fact]
        // public async Task AddAsync_AddsAndSavesSnapshot()
        // {
        //     var aggregate = new TestAggregate();
        //
        //     await _store.AddAsync<TestAggregate, Guid>(aggregate);
        //
        //     Assert.Contains(aggregate, _context.Set<TestAggregate>());
        // }

        public ValueTask DisposeAsync() => _context.DisposeAsync();

        private record TestAggregate : Aggregate;
    }
}
