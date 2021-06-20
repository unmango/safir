using System;
using Microsoft.Extensions.DependencyInjection;
using Safir.EventSourcing.DependencyInjection;
using Safir.EventSourcing.EntityFrameworkCore.DependencyInjection;
using Xunit;

namespace Safir.EventSourcing.EntityFrameworkCore.Tests.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        private readonly IServiceCollection _services = new ServiceCollection();
        
        [Fact]
        public void AddEntityFrameworkEventSourcing_AddsLibraryServices()
        {
            _services.AddEntityFrameworkEventSourcing();

            Assert.Contains(_services, x => x.ServiceType == typeof(ISafirEventSourcing));
        }
        
        [Fact]
        public void AddEntityFrameworkEventSourcing_AddsEventDbContext()
        {
            _services.AddEntityFrameworkEventSourcing();

            Assert.Contains(_services, x => x.ServiceType == typeof(EventSourcingContext));
        }
        
        [Fact]
        public void AddEntityFrameworkEventSourcing_AddsDbContextEventStore()
        {
            _services.AddEntityFrameworkEventSourcing();

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(IEventStore) &&
                x.ImplementationType == typeof(DbContextEventStore<EventSourcingContext>));

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(IEventStore<Guid>) &&
                x.ImplementationType == typeof(DbContextEventStore<EventSourcingContext>));
        }
        
        [Fact]
        public void AddEntityFrameworkEventSourcing_AddsDbContextSnapshotStore()
        {
            _services.AddEntityFrameworkEventSourcing();

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(ISnapshotStore) &&
                x.ImplementationType == typeof(DbContextSnapshotStore<EventSourcingContext>));
        }
        
        [Fact]
        public void AddEntityFrameworkEventSourcing_Generic_AddsLibraryServices()
        {
            _services.AddEntityFrameworkEventSourcing<TestContext>();

            Assert.Contains(_services, x => x.ServiceType == typeof(ISafirEventSourcing));
        }
        
        [Fact]
        public void AddEntityFrameworkEventSourcing_Generic_AddsDbContextEventStore()
        {
            _services.AddEntityFrameworkEventSourcing<TestContext>();

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(IEventStore) &&
                x.ImplementationType == typeof(DbContextEventStore<TestContext>));

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(IEventStore<Guid>) &&
                x.ImplementationType == typeof(DbContextEventStore<TestContext>));
        }
        
        [Fact]
        public void AddEntityFrameworkEventSourcing_Generic_AddsDbContextSnapshotStore()
        {
            _services.AddEntityFrameworkEventSourcing<TestContext>();

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(ISnapshotStore) &&
                x.ImplementationType == typeof(DbContextSnapshotStore<TestContext>));
        }

        [Fact]
        public void AddDbContextEventStore_AddsEventDbContext()
        {
            _services.AddDbContextEventStore();

            Assert.Contains(_services, x => x.ServiceType == typeof(EventSourcingContext));
        }
        
        [Fact]
        public void AddDbContextEventStore_AddsDbContextEventStore()
        {
            _services.AddDbContextEventStore();

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(IEventStore) &&
                x.ImplementationType == typeof(DbContextEventStore<EventSourcingContext>));

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(IEventStore<Guid>) &&
                x.ImplementationType == typeof(DbContextEventStore<EventSourcingContext>));
        }
        
        [Fact]
        public void AddDbContextEventStore_Generic_AddsDbContextEventStore()
        {
            _services.AddDbContextEventStore<TestContext>();

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(IEventStore) &&
                x.ImplementationType == typeof(DbContextEventStore<TestContext>));

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(IEventStore<Guid>) &&
                x.ImplementationType == typeof(DbContextEventStore<TestContext>));
        }

        [Fact]
        public void AddDbContextSnapshotStore_AddsEventDbContext()
        {
            _services.AddDbContextSnapshotStore();

            Assert.Contains(_services, x => x.ServiceType == typeof(EventSourcingContext));
        }
        
        [Fact]
        public void AddDbContextSnapshotStore_AddsDbContextSnapshotStore()
        {
            _services.AddDbContextSnapshotStore();

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(ISnapshotStore) &&
                x.ImplementationType == typeof(DbContextSnapshotStore<EventSourcingContext>));
        }
        
        [Fact]
        public void AddDbContextSnapshotStore_Generic_AddsDbContextSnapshotStore()
        {
            _services.AddDbContextSnapshotStore<TestContext>();

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(ISnapshotStore) &&
                x.ImplementationType == typeof(DbContextSnapshotStore<TestContext>));
        }
    }
}
