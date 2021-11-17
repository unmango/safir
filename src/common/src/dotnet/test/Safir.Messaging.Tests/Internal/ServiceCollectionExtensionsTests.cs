using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Safir.Messaging.DependencyInjection;
using Safir.Messaging.Internal;
using Safir.Messaging.Tests.Fakes;
using Xunit;

namespace Safir.Messaging.Tests.Internal
{
    public class ServiceCollectionExtensionsTests
    {
        private readonly IServiceCollection _services = new ServiceCollection();

        [Fact]
        public void GetRegisteredEventTypes_ReturnsEmptyWhenNoRegisteredHandlers()
        {
            var result = _services.GetRegisteredEventTypes();
            
            Assert.Empty(result);
        }

        [Fact]
        public void GetRegisteredEventTypes_ReturnsEventTypeWhenHandlerRegistered()
        {
            _services.AddEventHandler<MockEventHandler>();

            var result = _services.GetRegisteredEventTypes().ToList();
            
            Assert.NotEmpty(result);
            var type = Assert.Single(result);
            Assert.Equal(typeof(MockEvent), type);
        }
    }
}
