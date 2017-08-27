// <copyright file="ManagerPackage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager
{
    using Audio;
    using Command;
    using Query;
    using SimpleInjector;

    public static class ManagerPackage
    {
        public static void RegisterServices(Container container) {
            container.RegisterSingleton<MusicManager>();
            container.RegisterSingleton<IDeviceManager, DeviceManager>();
            container.RegisterSingleton<ISoundOutManager, SoundOutManager>();
            container.RegisterSingleton<IWaveSourceManager, WaveSourceManager>();

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
