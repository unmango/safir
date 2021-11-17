using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Moq.AutoMock;
using Safir.Agent.Protos;
using Safir.Agent.Services;
using Safir.Messaging.MediatR;
using Xunit;

namespace Safir.Agent.Tests.Services
{
    public class FileEventPublisherTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly FileEventPublisher _service;

        public FileEventPublisherTests()
        {
            _service = _mocker.CreateInstance<FileEventPublisher>();
        }

        [Fact]
        public async Task PublishesCreatedEventsWhenStarted()
        {
            SetupObservables();
            var watcher = _mocker.GetMock<IFileWatcher>();
            var publisher = _mocker.GetMock<IPublisher>();
            var subject = new Subject<FileSystemEventArgs>();
            watcher.SetupGet(x => x.Created).Returns(subject);
            const string name = "name";

            await _service.StartAsync(default);
            subject.OnNext(new(WatcherChangeTypes.Created, "/dir", name));
            
            publisher.Verify(x => x.Publish<INotification>(
                It.Is<Notification<FileCreated>>(e => e.Value.Path == name),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task PublishesChangedEventsWhenStarted()
        {
            SetupObservables();
            var watcher = _mocker.GetMock<IFileWatcher>();
            var publisher = _mocker.GetMock<IPublisher>();
            var subject = new Subject<FileSystemEventArgs>();
            watcher.SetupGet(x => x.Changed).Returns(subject);
            const string name = "name";

            await _service.StartAsync(default);
            subject.OnNext(new(WatcherChangeTypes.Changed, "/dir", name));
            
            publisher.Verify(x => x.Publish<INotification>(
                It.Is<Notification<FileChanged>>(e => e.Value.Path == name),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task PublishesDeletedEventsWhenStarted()
        {
            SetupObservables();
            var watcher = _mocker.GetMock<IFileWatcher>();
            var publisher = _mocker.GetMock<IPublisher>();
            var subject = new Subject<FileSystemEventArgs>();
            watcher.SetupGet(x => x.Deleted).Returns(subject);
            const string name = "name";

            await _service.StartAsync(default);
            subject.OnNext(new(WatcherChangeTypes.Deleted, "/dir", name));
            
            publisher.Verify(x => x.Publish<INotification>(
                It.Is<Notification<FileDeleted>>(e => e.Value.Path == name),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task PublishesRenamedEventsWhenStarted()
        {
            SetupObservables();
            var watcher = _mocker.GetMock<IFileWatcher>();
            var publisher = _mocker.GetMock<IPublisher>();
            var subject = new Subject<RenamedEventArgs>();
            watcher.SetupGet(x => x.Renamed).Returns(subject);
            const string name = "name", oldName = "old";

            await _service.StartAsync(default);
            subject.OnNext(new(WatcherChangeTypes.Renamed, "/dir", name, oldName));
            
            publisher.Verify(x => x.Publish<INotification>(
                It.Is<Notification<FileRenamed>>(e => e.Value.Path == name && e.Value.OldPath == oldName),
                It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task StopDisposesCreatedSubscription()
        {
            SetupObservables();
            var watcher = _mocker.GetMock<IFileWatcher>();
            var subject = new Subject<FileSystemEventArgs>();
            watcher.SetupGet(x => x.Created).Returns(subject);

            await _service.StartAsync(default);
            Assert.True(subject.HasObservers);
            
            await _service.StopAsync(default);
            Assert.False(subject.HasObservers);
        }

        [Fact]
        public async Task StopDisposesChangedSubscription()
        {
            SetupObservables();
            var watcher = _mocker.GetMock<IFileWatcher>();
            var subject = new Subject<FileSystemEventArgs>();
            watcher.SetupGet(x => x.Changed).Returns(subject);

            await _service.StartAsync(default);
            Assert.True(subject.HasObservers);
            
            await _service.StopAsync(default);
            Assert.False(subject.HasObservers);
        }

        [Fact]
        public async Task StopDisposesDeletedSubscription()
        {
            SetupObservables();
            var watcher = _mocker.GetMock<IFileWatcher>();
            var subject = new Subject<FileSystemEventArgs>();
            watcher.SetupGet(x => x.Deleted).Returns(subject);

            await _service.StartAsync(default);
            Assert.True(subject.HasObservers);
            
            await _service.StopAsync(default);
            Assert.False(subject.HasObservers);
        }

        [Fact]
        public async Task StopDisposesRenamedSubscription()
        {
            SetupObservables();
            var watcher = _mocker.GetMock<IFileWatcher>();
            var subject = new Subject<RenamedEventArgs>();
            watcher.SetupGet(x => x.Renamed).Returns(subject);

            await _service.StartAsync(default);
            Assert.True(subject.HasObservers);
            
            await _service.StopAsync(default);
            Assert.False(subject.HasObservers);
        }

        private void SetupObservables()
        {
            // The reactive extensions expect a real sequence, so give them a default
            var watcher = _mocker.GetMock<IFileWatcher>();
            watcher.SetupGet(x => x.Created).Returns(Observable.Empty<FileSystemEventArgs>());
            watcher.SetupGet(x => x.Changed).Returns(Observable.Empty<FileSystemEventArgs>());
            watcher.SetupGet(x => x.Deleted).Returns(Observable.Empty<FileSystemEventArgs>());
            watcher.SetupGet(x => x.Renamed).Returns(Observable.Empty<RenamedEventArgs>());
        }
    }
}
