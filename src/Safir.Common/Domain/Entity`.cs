namespace Safir.Common.Domain
{
    public abstract class Entity<T> : Entity
        where T : struct
    {
        public T Id { get; protected internal set; }
    }
}
