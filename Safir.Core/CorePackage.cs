using SimpleInjector;

namespace Safir.Core
{
    using Settings;

    public static class CorePackage
    {
        public static void RegisterServices(Container container)
        {
            container.Register<ISettingStore, XmlSettingsStore>();
        }
    }
}
