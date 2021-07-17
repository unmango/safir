using System;

namespace Safir.Messaging.MediatR.Tests.Fakes
{
    // ReSharper disable once CA1067
    public record FakeEvent : IEvent
    {
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public DateTime Occurred { get; }
    }
}
