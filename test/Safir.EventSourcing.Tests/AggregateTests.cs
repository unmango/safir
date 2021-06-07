using System;
using System.Collections.Generic;
using System.Linq;
using Safir.Messaging;
using Xunit;

namespace Safir.EventSourcing.Tests
{
    public class AggregateTests
    {
        [Fact]
        public void Apply_SetsVersion()
        {
            const int version = 69;
            IAggregate aggregate = new TestAggregate();
            var @event = new MockEvent { Version = version };

            aggregate.Apply(@event);

            Assert.Equal(version, aggregate.Version);
        }

        [Fact]
        public void Apply_KeepsHighestVersion()
        {
            const int version = 420;
            IAggregate aggregate = new TestAggregate();
            var event1 = new MockEvent { Version = version };
            var event2 = new MockEvent { Version = version - 69 };

            aggregate.Apply(event1);
            aggregate.Apply(event2);

            Assert.Equal(version, aggregate.Version);
        }

        [Fact]
        public void DequeueEvents_DequeuesAllEvents()
        {
            var events = new[] {
                new MockEvent(),
                new MockEvent()
            };
            var aggregate = new TestAggregate(events);
            
            Assert.NotEmpty(aggregate.Events);
            Assert.Equal(events.Length, aggregate.Events.Count());

            var dequeued = aggregate.DequeueEvents().ToList();
            
            Assert.NotEmpty(dequeued);
            Assert.Empty(aggregate.Events);
            Assert.Equal(events.Length, dequeued.Count);
        }

        private record MockEvent : IEvent
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public DateTime Occurred { get; }

            public int Version { get; init; }
        }

        private record TestAggregate : Aggregate
        {
            public TestAggregate() { }

            public TestAggregate(IEnumerable<IEvent> events)
            {
                foreach (var @event in events) Enqueue(@event);
            }
        }
    }
}
