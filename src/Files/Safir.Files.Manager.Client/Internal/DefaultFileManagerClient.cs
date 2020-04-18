namespace Safir.Files.Manager.Client.Internal
{
    internal class DefaultFileManagerClient : IFileManagerClient
    {
        private readonly Greeter.GreeterClient _client;

        public DefaultFileManagerClient(Greeter.GreeterClient client)
        {
            _client = client;
        }
    }
}
