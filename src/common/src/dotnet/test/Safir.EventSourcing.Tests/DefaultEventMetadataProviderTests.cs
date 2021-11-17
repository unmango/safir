using System;
using System.Threading.Tasks;
using Moq.AutoMock;
using Safir.Messaging;
using Xunit;

namespace Safir.EventSourcing.Tests
{
    public class DefaultEventMetadataProviderTests
    {
        private readonly AutoMocker _mocker = new();
        private readonly DefaultEventMetadataProvider _provider;

        public DefaultEventMetadataProviderTests()
        {
            _provider = _mocker.CreateInstance<DefaultEventMetadataProvider>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void GetTypeDiscriminator_ReturnsAssemblyQualifiedName(int version)
        {
            var @event = new MockEvent();

            var result = _provider.GetTypeDiscriminator(@event, version);

            Assert.Equal(
                "Safir.EventSourcing.Tests.DefaultEventMetadataProviderTests+MockEvent, Safir.EventSourcing.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task GetTypeDiscriminatorAsync_ReturnsAssemblyQualifiedName(int version)
        {
            var @event = new MockEvent();

            var result = await _provider.GetTypeDiscriminatorAsync(@event, version);

            Assert.Equal(
                "Safir.EventSourcing.Tests.DefaultEventMetadataProviderTests+MockEvent, Safir.EventSourcing.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void GetType_ReturnsAssemblyType(int version)
        {
            var result = _provider.GetType(
                "Safir.EventSourcing.Tests.DefaultEventMetadataProviderTests+MockEvent, Safir.EventSourcing.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                version);

            Assert.Equal(typeof(MockEvent), result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task GetTypeAsync_ReturnsAssemblyQualifiedName(int version)
        {
            var result = await _provider.GetTypeAsync(
                "Safir.EventSourcing.Tests.DefaultEventMetadataProviderTests+MockEvent, Safir.EventSourcing.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                version);

            Assert.Equal(typeof(MockEvent), result);
        }

        // ReSharper disable once CA1067
        private record MockEvent : IEvent
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public DateTime Occurred { get; }
        }
    }
}
