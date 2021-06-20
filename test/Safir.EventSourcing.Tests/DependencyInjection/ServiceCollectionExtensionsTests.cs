using Microsoft.Extensions.DependencyInjection;
using Safir.Common;
using Safir.EventSourcing.DependencyInjection;
using Xunit;

namespace Safir.EventSourcing.Tests.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        private readonly IServiceCollection _services = new ServiceCollection();

        [Fact]
        public void AddEventSourcing_AddsDefaultSerializer()
        {
            _services.AddEventSourcing();

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(ISerializer) &&
                x.ImplementationType == typeof(DefaultSerializer));
        }

        [Fact]
        public void AddEventSourcing_AddsEventSerializer()
        {
            _services.AddEventSourcing();

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(IEventSerializer) &&
                x.ImplementationType == typeof(DefaultEventSerializer));
        }

        [Fact]
        public void AddEventSourcing_AddsDefaultEventMetadataProvider()
        {
            _services.AddEventSourcing();

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(IEventMetadataProvider) &&
                x.ImplementationType == typeof(DefaultEventMetadataProvider));
        }

        [Fact]
        public void AddEventSourcing_AddsDefaultAggregateStore()
        {
            _services.AddEventSourcing();

            Assert.Contains(_services, x =>
                x.ServiceType == typeof(IAggregateStore) &&
                x.ImplementationType == typeof(DefaultAggregateStore));
        }

        [Fact]
        public void AddEventSourcing_AddsMarkerInterface()
        {
            _services.AddEventSourcing();

            Assert.Contains(_services, x => x.ServiceType == typeof(ISafirEventSourcing));
        }
    }
}
