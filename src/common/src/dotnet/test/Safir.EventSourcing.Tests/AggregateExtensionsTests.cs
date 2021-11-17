using System;
using Moq;
using Safir.Messaging;
using Xunit;

namespace Safir.EventSourcing.Tests
{
    public class AggregateExtensionsTests
    {
        private readonly Mock<IAggregate> _aggregate = new();

        [Fact]
        public void Apply_AppliesAllEvents()
        {
            var event1 = new MockEvent(DateTime.Now);
            var event2 = new MockEvent(DateTime.UtcNow);
            
            _aggregate.Object.Apply(new[] { event1, event2 });
            
            _aggregate.Verify(x => x.Apply(event1), Times.Once);
            _aggregate.Verify(x => x.Apply(event2), Times.Once);
        }

        private record MockEvent(DateTime Occurred) : IEvent;
    }
}
