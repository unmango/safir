namespace Cli.Services.Installation
{
    internal interface ISynchronousSourceInstaller<T> : ISourceInstaller
        where T : IServiceSource
    {
        ISourceInstalled GetInstalled(SourceContext<T> context);

        IServiceUpdate GetUpdate(SourceContext<T> context);

        void Install(SourceContext<T> context);

        void Update(SourceContext<T> context);
    }
}
