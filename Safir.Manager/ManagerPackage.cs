using SimpleInjector;

namespace Safir.Manager
{
    using Command;
    using Query;

    public static class ManagerPackage
    {
        public static void RegisterServices(Container container) {
            container.Register(typeof(IQueryHandler<,>), new[] { typeof(IQueryHandler<,>).Assembly });
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(ValidationQueryHandlerDecorator<,>),
                context => ShouldQueryHandlerBeValidated(context.ServiceType));
            container.RegisterSingleton<IQueryProcessor, QueryProcessor>();

            container.Register(typeof(ICommandHandler<>), new[] { typeof(ICommandHandler<>).Assembly });
        }

        private static bool ShouldQueryHandlerBeValidated(object serviceType) {
            return false;
        }
    }
}
