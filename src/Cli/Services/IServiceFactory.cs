namespace Cli.Services
{
    internal interface IServiceFactory
    {
        IService Create(ServiceEntry service);
    }
}
