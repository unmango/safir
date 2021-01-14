namespace Cli.Services
{
    internal interface IServiceStatus
    {
        // TODO: Probably add string overload and forward `Find` call to registry
        ServiceStatus GetStatus(IService service);
    }
}
