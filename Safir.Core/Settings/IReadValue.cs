namespace Safir.Core.Settings
{
    public interface IReadValue<out T>
    {
        T Get(string key);
    }
}
