namespace Safir.Core.Settings
{
    public interface ISettingStore : IReadValue<string>, IWriteValue<string>
    {
        void Load();
        void Save();
    }
}
