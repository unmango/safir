namespace Safir.Core.Settings
{
    public interface IWriteValue<in T>
    {
        void Set(string key, T value);
    }
}
