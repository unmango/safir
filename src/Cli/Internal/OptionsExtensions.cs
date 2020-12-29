namespace Cli.Internal
{
    internal static class OptionsExtensions
    {
        public static string GetInstallationDirectory(this Options options)
        {
            return options.Service.GetInstallationDirectory();
        }

        public static string GetInstallationDirectory(this Service options)
        {
            return options.CustomDirectory ?? Service.Directory;
        }
    }
}
