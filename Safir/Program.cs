using Safir.Manager;
using Safir.ViewModels;
using Safir.Views;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var container = Bootstrap();

            // Any additional other configuration, e.g. of your desired MVVM toolkit.


            RunApplication(container);
        }

        private static Container Bootstrap()
        {
            var container = new Container();

            // Register Types
            container.Register<IDbContext, MusicContext>();

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
