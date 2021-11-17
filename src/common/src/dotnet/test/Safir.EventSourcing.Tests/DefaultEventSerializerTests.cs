using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Safir.Common;
using Safir.Messaging;
using Xunit;

namespace Safir.EventSourcing.Tests
{
    public class DefaultEventSerializerTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly Mock<ISerializer> _serializer;
        private readonly Mock<IEventMetadataProvider> _metadataProvider;
        private readonly DefaultEventSerializer _eventSerializer;

        public DefaultEventSerializerTests()
        {
            _serializer = _mocker.GetMock<ISerializer>();
            _metadataProvider = _mocker.GetMock<IEventMetadataProvider>();
            _eventSerializer = _mocker.CreateInstance<DefaultEventSerializer>();
        }

        [Fact]
        public async Task SerializeAsync_SerializesAsEventEntity()
        {
            var id = Guid.NewGuid();
            const string type = "type";
            IEvent value = new MockEvent();
            
            _metadataProvider.Setup(
                x => x.GetTypeDiscriminatorAsync(value, It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(type);

            var result = await _eventSerializer.SerializeAsync(id, value);
            
            _metadataProvider.Verify();
            _serializer.Verify(
                x => x.SerializeAsync(It.IsAny<IBufferWriter<byte>>(), value, It.IsAny<CancellationToken>()));
            
            Assert.Equal(id, result.AggregateId);
            Assert.Equal(type, result.Type);
            Assert.Equal(value.Occurred, result.Occurred);
            Assert.Equal(value.CorrelationId, result.Metadata.CorrelationId);
            Assert.Equal(value.CausationId, result.Metadata.CausationId);
            Assert.Equal(value.Version, result.Version);
        }

        [Fact]
        public async Task DeserializeAsync_DeserializesEntityAsEvent()
        {
            const string discriminator = "type";
            var type = typeof(MockEvent);
            const int version = 69;
            var value = new Event(Guid.NewGuid(), discriminator, Array.Empty<byte>(), DateTime.Now, new Metadata(), version);
            _metadataProvider.Setup(x => x.GetTypeAsync(discriminator, version, It.IsAny<CancellationToken>()))
                .ReturnsAsync(type);

            await _eventSerializer.DeserializeAsync(value);
            
            _metadataProvider.Verify();
            _serializer.Verify(x => x.DeserializeAsync(type, value.Data, It.IsAny<CancellationToken>()));
        }

        private record MockEvent : IEvent
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public DateTime Occurred { get; }
        }
    }
}
