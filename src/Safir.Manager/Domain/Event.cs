using System;

namespace Safir.Manager.Domain
{
    public record Event<T>(T Id, DateTime Occurred, string Serialized, int Version);
}
