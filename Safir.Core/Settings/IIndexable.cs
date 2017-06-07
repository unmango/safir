namespace Safir.Core.Settings
{
    public interface IIndexable<T>
    {
        T this[string index] { get; set; }
    }
}
