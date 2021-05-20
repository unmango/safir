using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;
using Safir.Messaging.Tests.Fakes;
using Xunit;

namespace Safir.Messaging.Tests
{
    public class SubscriptionManagerTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IServiceProvider> _services;
        private readonly Mock<IEventBus> _eventBus;
        private readonly SubscriptionManager<MockEvent> _manager;
        private readonly CancellationToken _cancellationToken = default;

        public SubscriptionManagerTests()
        {
            _services = _mocker.GetMock<IServiceProvider>();
            
            var scope = _mocker.GetMock<IServiceScope>();
            scope.SetupGet(x => x.ServiceProvider).Returns(_services.Object);
            
            var factory = _mocker.GetMock<IServiceScopeFactory>();
            factory.Setup(x => x.CreateScope()).Returns(scope.Object);

            _services.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(factory.Object);
            
            _eventBus = _mocker.GetMock<IEventBus>();
            _manager = _mocker.CreateInstance<SubscriptionManager<MockEvent>>();
        }

        [Fact]
        public async Task StartAsync_StartsWithNoHandlers()
        {
            await _manager.StartAsync(_cancellationToken);

            _eventBus.Verify(
                x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()),
                Times.Once);
            _services.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task StartAsync_StartsWithSingleHandler()
        {
            var handler = _mocker.GetMock<IEventHandler<MockEvent>>();
            _services.Setup(x => x.GetService(typeof(IEnumerable<IEventHandler>))).Returns(new[] { handler.Object });
            var expectedEvent = new MockEvent();
            IObserver<MockEvent>? observer = null;
            _eventBus.Setup(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()))
                .Callback<IObserver<MockEvent>, CancellationToken>((x, _) => observer = x);

            await _manager.StartAsync(_cancellationToken);

            _eventBus.Verify(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), _cancellationToken));
            Assert.NotNull(observer);

            observer?.OnNext(expectedEvent);

            handler.Verify(x => x.HandleAsync(expectedEvent, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task StartAsync_StartsWhenHandlerFailsToSubscribe()
        {
            var handler1 = _mocker.GetMock<IEventHandler<MockEvent>>();
            var handler2 = _mocker.GetMock<IEventHandler<MockEvent>>();
            _services.Setup(x => x.GetService(typeof(IEnumerable<IEventHandler>))).Returns(new[] { handler1.Object, handler2.Object });
            _eventBus.SetupSequence(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Disposable.Empty)
                .ThrowsAsync(new Exception("Test exception"));
            IObserver<MockEvent>? observer = null;
            var message = new MockEvent();
            _eventBus.Setup(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()))
                .Callback<IObserver<MockEvent>, CancellationToken>((x, _) => observer = x);

            await _manager.StartAsync(_cancellationToken);

            _eventBus.Verify(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(observer);

            observer?.OnNext(message);

            handler1.Verify(x => x.HandleAsync(It.IsAny<MockEvent>(), It.IsAny<CancellationToken>()));
            handler2.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task StartAsync_ReSubscribesWhenHandlerThrows()
        {
            var handler = _mocker.GetMock<IEventHandler<MockEvent>>();
            handler.SetupSequence(x => x.HandleAsync(It.IsAny<MockEvent>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"))
                .Returns(Task.CompletedTask);
            _services.Setup(x => x.GetService(typeof(IEnumerable<IEventHandler>))).Returns(new[] { handler.Object });
            IObserver<MockEvent>? observer = null;
            var message = new MockEvent();
            _eventBus.Setup(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()))
                .Callback<IObserver<MockEvent>, CancellationToken>((x, _) => observer = x);

            await _manager.StartAsync(_cancellationToken);

            _eventBus.Verify(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()));
            Assert.NotNull(observer);

            observer?.OnNext(message);
            observer?.OnNext(message);

            handler.Verify(x => x.HandleAsync(It.IsAny<MockEvent>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task StopAsync_StopsWithNoHandlers()
        {
            await _manager.StartAsync(_cancellationToken);
            // Shouldn't do anything
            await _manager.StopAsync(_cancellationToken);
        }

        [Fact]
        public async Task StopAsync_StopsWithSingleHandler()
        {
            var handler = _mocker.GetMock<IEventHandler<MockEvent>>();
            _mocker.Use(typeof(IEnumerable<IEventHandler>), new[] { handler.Object });
            var manager = _mocker.CreateInstance<SubscriptionManager<MockEvent>>();
            var disposable = _mocker.GetMock<IDisposable>();
            _eventBus.Setup(x => x.SubscribeAsync(It.IsAny<IObserver<MockEvent>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(disposable.Object);

            await manager.StartAsync(_cancellationToken);
            await manager.StopAsync(_cancellationToken);

            disposable.Verify(x => x.Dispose());
        }
    }
}
