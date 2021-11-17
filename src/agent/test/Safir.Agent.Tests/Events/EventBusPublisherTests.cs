using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Safir.Agent.Events;
using Safir.Agent.Protos;
using Safir.Messaging;
using Safir.Messaging.MediatR;
using Xunit;

namespace Safir.Agent.Tests.Events
{
    public class EventBusPublisherTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<IEventBus> _eventBus;
        private readonly EventBusPublisher _publisher;
        private readonly CancellationToken _cancellationToken = default;

        public EventBusPublisherTests()
        {
            _eventBus = _mocker.GetMock<IEventBus>();
            _publisher = _mocker.CreateInstance<EventBusPublisher>();
        }

        [Fact]
        public async Task HandleFileCreated_PublishesToEventBus()
        {
            var notification = CreateNotification<FileCreated>();

            await _publisher.Handle(notification, _cancellationToken);
            
            _eventBus.Verify(x => x.PublishAsync(notification.Value, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task HandleFileCreated_HandlesEventBusException()
        {
            var notification = CreateNotification<FileCreated>();
            _eventBus
                .Setup(x => x.PublishAsync(It.IsAny<FileCreated>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Bad things happening"));

            await _publisher.Handle(notification, _cancellationToken);
        }

        [Fact]
        public async Task HandleFileChanged_PublishesToEventBus()
        {
            var notification = CreateNotification<FileChanged>();

            await _publisher.Handle(notification, _cancellationToken);
            
            _eventBus.Verify(x => x.PublishAsync(notification.Value, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task HandleFileChanged_HandlesEventBusException()
        {
            var notification = CreateNotification<FileChanged>();
            _eventBus
                .Setup(x => x.PublishAsync(It.IsAny<FileChanged>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Bad things happening"));

            await _publisher.Handle(notification, _cancellationToken);
        }

        [Fact]
        public async Task HandleFileDeleted_PublishesToEventBus()
        {
            var notification = CreateNotification<FileDeleted>();

            await _publisher.Handle(notification, _cancellationToken);
            
            _eventBus.Verify(x => x.PublishAsync(notification.Value, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task HandleFileDeleted_HandlesEventBusException()
        {
            var notification = CreateNotification<FileDeleted>();
            _eventBus
                .Setup(x => x.PublishAsync(It.IsAny<FileDeleted>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Bad things happening"));

            await _publisher.Handle(notification, _cancellationToken);
        }

        [Fact]
        public async Task HandleFileRenamed_PublishesToEventBus()
        {
            var notification = CreateNotification<FileRenamed>();

            await _publisher.Handle(notification, _cancellationToken);
            
            _eventBus.Verify(x => x.PublishAsync(notification.Value, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task HandleFileRenamed_HandlesEventBusException()
        {
            var notification = CreateNotification<FileRenamed>();
            _eventBus
                .Setup(x => x.PublishAsync(It.IsAny<FileRenamed>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Bad things happening"));

            await _publisher.Handle(notification, _cancellationToken);
        }

        private static Notification<T> CreateNotification<T>()
            where T : IEvent, new()
            => new(new());
    }
}
