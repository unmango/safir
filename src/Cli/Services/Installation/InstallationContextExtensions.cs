namespace Cli.Services.Installation
{
    internal static class InstallationContextExtensions
    {
        public static T WithProperty<T>(this T obj, object key, object value)
            where T : InstallationContext
            => obj with {
                Properties = obj.Properties.SetItem(key, value)
            };
    }
}
