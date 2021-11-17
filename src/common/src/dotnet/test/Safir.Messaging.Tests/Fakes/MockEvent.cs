using System;

namespace Safir.Messaging.Tests.Fakes
{
    public class MockEvent : IEvent
    {
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public DateTime Occurred { get; }
    }
}
