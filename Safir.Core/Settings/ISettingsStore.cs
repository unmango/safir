namespace Safir.Core.Settings
{
    public interface ISettingStore : IReadValue<string>, IWriteValue<string>, IIndexable<string>
    {
        void Load();
        void Save();
    }
}
