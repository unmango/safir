using SimpleInjector;
using System;

namespace Safir
{
    using Core;
    using Core.Application;
    using Manager;
    using ViewModels;
    using Views;

    static class Program
    {
        [STAThread]
        static void Main()
        {
            var container = Bootstrap();

            // Any additional other configuration, e.g. of your desired MVVM toolkit.
            // I don't know how to do this ↑ yet

            RunApplication(container);
        }

        private static Container Bootstrap()
        {
            var container = new Container();

            // TODO: Register logger

            // Register Types
            container.Register<IAppMeta>(() => new ApplicationMeta("Safir"));
            CorePackage.RegisterServices(container);
            ManagerPackage.RegisterServices(container);

            // Register windows and view models
            container.Register<MainView>();
            container.Register<MainViewModel>();

            container.Verify();

            return container;
        }

        private static void RunApplication(Container container)
        {
            try
            {
                var app = new App();
                var mainWindow = container.GetInstance<MainView>();
                app.Run(mainWindow);
            }
            catch (Exception ex)
            {
                // Log exception and exit
            }
        }
    }
}
