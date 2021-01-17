namespace Cli.Services.Installation.Installers
{
    internal static class InstallationContextExtensions
    {
        public static TContext MarkInstalled<TContext, TSource>(this TContext context, TSource source)
            where TContext : InstallationContext
            where TSource : IServiceSource
            => context.SetInstalled(source, true);

        private static TContext SetInstalled<TContext, TSource>(this TContext context, TSource source, bool status)
            where TContext : InstallationContext
            where TSource : IServiceSource
            => context.WithProperty(SourceInstalled.Key(source), status);
    }
}
