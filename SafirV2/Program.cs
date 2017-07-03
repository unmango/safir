using Caliburn.Micro;
using SimpleInjector;
using System;

namespace Safir
{
    using Core;
    using Core.Application;
    using Core.Helpers;
    using Manager;
    using ViewModels;
    using Views;

    [Obsolete]
    static class Program
    {
        private const string APPNAME = "Safir";

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

            //container.RegisterSingleton(LogManager.GetLogger(typeof(object)));
            //container.RegisterConditional(typeof(ILog),
            //    c => typeof(Log4NetAdapter<>).MakeGenericType(c.Consumer.ImplementationType),
            //    Lifestyle.Singleton,
            //    c => true);

            // Register Types
            container.RegisterSingleton<IAppMeta>(() => new ApplicationMeta(APPNAME));

            CorePackage.RegisterServices(container);
            ManagerPackage.RegisterServices(container);

            // Register windows and view models
            container.Register<IWindowManager, WindowManager>();
            container.RegisterSingleton<IEventAggregator, EventAggregator>();

            container.Register<MainView>();
            container.Register<MainViewModel>();

            container.Register<MainMenuView>();
            container.Register<MainMenuViewModel>();

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
                //LogHelper.GetLogger().Fatal(ex);
            }
        }
    }
}
