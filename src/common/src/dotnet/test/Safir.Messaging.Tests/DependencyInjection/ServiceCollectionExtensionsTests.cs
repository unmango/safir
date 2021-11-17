using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Safir.Agent.Protos;
using Safir.Messaging.Configuration;
using Safir.Messaging.DependencyInjection;
using Safir.Messaging.Tests.Fakes;
using Safir.Redis.Configuration;
using Xunit;

namespace Safir.Messaging.Tests.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        private readonly ServiceCollection _services = new();
        
        [Fact]
        public void AddEventHandler_AddsSubscriptionManager()
        {
            var services = _services.AddEventHandler<MockEventHandler>()
                .BuildServiceProvider();

            var hostedServices = services.GetService<IEnumerable<IHostedService>>()?.ToList();
            
            Assert.NotNull(hostedServices);
            Assert.Single(hostedServices!.OfType<SubscriptionManager<FileCreated>>());
            Assert.Single(hostedServices!.OfType<SubscriptionManager<FileChanged>>());
            Assert.Single(hostedServices!.OfType<SubscriptionManager<FileDeleted>>());
            Assert.Single(hostedServices!.OfType<SubscriptionManager<FileRenamed>>());
        }

        [Fact]
        public void AddEventHandler_AddsEventHandler()
        {
            var services = _services.AddEventHandler<MockEventHandler>()
                .BuildServiceProvider();

            var handlers = services.GetService<IEnumerable<IEventHandler>>();
            
            Assert.NotNull(handlers);
            Assert.Single(handlers!);
        }

        [Fact]
        public void AddEventHandler_AddsMultipleEventHandlers()
        {
            var services = _services
                .AddEventHandler<MockEventHandler>()
                .AddEventHandler<MockEventHandler2>()
                .BuildServiceProvider();

            var handlers = services.GetService<IEnumerable<IEventHandler>>();
            
            Assert.NotNull(handlers);
            Assert.Equal(2, handlers!.Count());
        }

        [Fact]
        public void AddEventHandler_AddsDifferentEventHandlers()
        {
            var services = _services
                .AddEventHandler<MockEventHandler>()
                .AddEventHandler<DifferentEventHandler>()
                .BuildServiceProvider();

            var handlers = services.GetService<IEnumerable<IEventHandler>>()?.ToList();
            
            Assert.NotNull(handlers);
            Assert.Equal(2, handlers!.Count);
            Assert.Single(handlers!.OfType<MockEventHandler>());
            Assert.Single(handlers!.OfType<DifferentEventHandler>());
        }

        [Fact]
        public void AddSafirMessaging_AddsMessagingOptions()
        {
            var services = _services.AddSafirMessaging()
                .BuildServiceProvider();

            var options = services.GetService<IOptions<MessagingOptions>>();
            
            Assert.NotNull(options);
        }

        [Fact]
        public void AddSafirMessaging_AddsRedisEventBus()
        {
            var services = _services.AddSafirMessaging()
                .BuildServiceProvider();

            var bus = services.GetService<IEventBus>();
            
            Assert.NotNull(bus);
            Assert.IsType<RedisEventBus>(bus);
        }

        [Fact]
        public void AddSafirMessaging_AddsDefaultTypedEventBus()
        {
            var services = _services.AddSafirMessaging()
                .BuildServiceProvider();

            var bus = services.GetService<IEventBus<MockEvent>>();
            
            Assert.NotNull(bus);
            Assert.IsType<DefaultTypedEventBus<MockEvent>>(bus);
        }

        [Fact]
        public void AddSafirMessaging_ConfiguresRedisOptions()
        {
            var services = _services.AddSafirMessaging()
                .BuildServiceProvider();

            var configureOptions = services.GetService<IConfigureOptions<RedisOptions>>();
            
            Assert.NotNull(configureOptions);
        }

        [UsedImplicitly]
        private class MockEventHandler2 : IEventHandler<MockEvent>
        {
            public Task HandleAsync(MockEvent message, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }
        
        // ReSharper disable once ClassNeverInstantiated.Local
        private class DifferentEvent : IEvent
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public DateTime Occurred { get; }
        }

        private class DifferentEventHandler : IEventHandler<DifferentEvent>
        {
            public Task HandleAsync(DifferentEvent message, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }
    }
}
