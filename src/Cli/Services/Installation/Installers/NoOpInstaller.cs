namespace Cli.Services.Installation.Installers
{
    internal sealed class NoOpInstaller : ISourceInstaller
    {
        public static readonly NoOpInstaller Value = new();

        private NoOpInstaller() { }
        
        public bool AppliesTo(ISourceContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
