using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging.Tests.Fakes;
using Xunit;

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local

namespace Safir.Messaging.Tests
{
    public class EventHandlerExtensionsTests
    {
        [Fact]
        public void GetEventTypes_GetsSingleGenericHandlerEventType()
        {
            var handler = new MockEventHandler();

            var types = handler.GetEventTypes();

            Assert.NotNull(types);
            var item = Assert.Single(types);
            Assert.Equal(typeof(MockEvent), item);
        }

        [Fact]
        public void GetEventTypes_ReturnsEmptyWhenHandlerIsNotGeneric()
        {
            var handler = new NonGenericHandler();

            var types = handler.GetEventTypes();

            Assert.Empty(types);
        }

        [Fact]
        public void GetEventTypes_ReturnsAllImplementationEventTypes()
        {
            var handler = new MultiImplHandler();

            var types = handler.GetEventTypes();

            Assert.NotNull(types);
            Assert.Collection(
                types,
                x => Assert.Equal(typeof(MockEvent), x),
                x => Assert.Equal(typeof(MockEvent2), x));
        }

        [Fact]
        public void GetEventTypes_ReturnsEventTypeWhenClassImplementsNonGenericHandler()
        {
            var handler = new SingleAndNonGenericImpl();

            var types = handler.GetEventTypes();

            Assert.NotNull(types);
            Assert.Single(types);
        }

        [Fact]
        public void GroupByEvent_GroupsSingleHandlerBySingleEventType()
        {
            var handler = new MockEventHandler();
            var handlers = new[] { handler };

            var grouped = handlers.GroupByEvent();

            Assert.NotNull(grouped);
            var item = Assert.Single(grouped);
            Assert.Equal(typeof(MockEvent), item!.Key);
            var value = Assert.Single(item);
            Assert.Same(handler, value);
        }

        [Fact]
        public void GroupByEvent_GroupsSingleHandlerByMultipleEventTypes()
        {
            var handler = new MultiImplHandler();
            var handlers = new[] { handler };

            var grouped = handlers.GroupByEvent();

            Assert.NotNull(grouped);
            Assert.Collection(
                grouped,
                x => AssertGrouping(x, typeof(MockEvent), handler),
                x => AssertGrouping(x, typeof(MockEvent2), handler));

            [AssertionMethod]
            static void AssertGrouping(
                IGrouping<Type, IEventHandler> grouping,
                Type expectedType,
                IEventHandler expectedHandler)
            {
                Assert.NotNull(grouping.Key);
                Assert.Equal(expectedType, grouping.Key);
                var actualHandler = Assert.Single(grouping);
                Assert.Same(expectedHandler, actualHandler);
            }
        }

        [Fact]
        public void GroupByEvent_ReturnsEmptyWhenNoGenericHandlers()
        {
            var handlers = new[] { new NonGenericHandler() };

            var grouped = handlers.GroupByEvent();

            Assert.NotNull(grouped);
            Assert.Empty(grouped);
        }

        [Fact]
        public void GroupByEvent_GroupsMultipleHandlersBySingleEventType()
        {
            var handler1 = new MockEventHandler();
            var handler2 = new MockEventHandler();
            var handlers = new[] { handler1, handler2 };

            var grouped = handlers.GroupByEvent();

            Assert.NotNull(grouped);
            var item = Assert.Single(grouped);
            Assert.Equal(typeof(MockEvent), item!.Key);
            Assert.Collection(
                item,
                x => Assert.Same(handler1, x),
                x => Assert.Same(handler2, x));
        }

        [Fact]
        public void GroupByEvent_GroupsMultipleHandlersByMultipleEventTypes()
        {
            var handler1 = new MockEventHandler();
            var handler2 = new MockEventHandler2();
            var handler3 = new MockEventHandler();
            var handler4 = new MockEventHandler2();
            var handlers = new IEventHandler[] { handler1, handler2, handler3, handler4 };

            var grouped = handlers.GroupByEvent();

            Assert.NotNull(grouped);
            Assert.Collection(
                grouped,
                x => AssertGrouping(x, typeof(MockEvent), handler1, handler3),
                x => AssertGrouping(x, typeof(MockEvent2), handler2, handler4));

            [AssertionMethod]
            static void AssertGrouping(
                IGrouping<Type, IEventHandler> grouping,
                Type expectedType,
                IEventHandler expectedHandler1,
                IEventHandler expectedHandler2)
            {
                Assert.NotNull(grouping.Key);
                Assert.Equal(expectedType, grouping.Key);

                Assert.Collection(
                    grouping,
                    x => Assert.Same(expectedHandler1, x),
                    x => Assert.Same(expectedHandler2, x));
            }
        }

        [Fact]
        public void GroupByEvent_GroupsMultipleImplHandlerByMultipleEventType()
        {
            var handler = new MultiImplHandler();
            var handlers = new[] { handler };

            var grouped = handlers.GroupByEvent();

            Assert.NotNull(grouped);
            Assert.Collection(
                grouped,
                x => AssertGrouping(x, typeof(MockEvent), handler),
                x => AssertGrouping(x, typeof(MockEvent2), handler));

            [AssertionMethod]
            static void AssertGrouping(
                IGrouping<Type, IEventHandler> grouping,
                Type expectedType,
                IEventHandler expectedHandler)
            {
                Assert.NotNull(grouping.Key);
                Assert.Equal(expectedType, grouping.Key);
                var actualHandler = Assert.Single(grouping);
                Assert.Same(expectedHandler, actualHandler);
            }
        }

        [Fact]
        public void IsGenericHandler_ReturnsTrueWhenConcreteTypeIsGenericHandler()
        {
            var handler = new MockEventHandler();

            var result = handler.IsGenericHandler();

            Assert.True(result);
        }

        [Fact]
        public void IsGenericHandler_ReturnsFalseWhenConcreteTypeIsNotGenericHandler()
        {
            var handler = new NonGenericHandler();

            var result = handler.IsGenericHandler();

            Assert.False(result);
        }

        // ReSharper disable once CA1067
        private record MockEvent2 : IEvent
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public DateTime Occurred { get; }
        }

        private class MockEventHandler2 : IEventHandler<MockEvent2>
        {
            public Task HandleAsync(MockEvent2 message, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }

        private class NonGenericHandler : IEventHandler
        {
        }

        private class MultiImplHandler : IEventHandler<MockEvent>, IEventHandler<MockEvent2>
        {
            public Task HandleAsync(MockEvent message, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task HandleAsync(MockEvent2 message, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }

        // ReSharper disable once RedundantExtendsListEntry
        private class SingleAndNonGenericImpl : IEventHandler<MockEvent>, IEventHandler
        {
            public Task HandleAsync(MockEvent message, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }
    }
}
