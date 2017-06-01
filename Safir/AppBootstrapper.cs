using Caliburn.Micro;
using Caliburn.Micro.Logging.log4net;
using Safir.ViewModels;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Safir
{
    using Core;
    using Core.Application;
    using Core.Settings;
    using Manager;

    internal class AppBootstrapper : BootstrapperBase
    {
        private const string APPNAME = "Safir";
        private static readonly Container _container = new Container();

        public AppBootstrapper() {
            LogManager.GetLog = type => new log4netLogger(type);
            Initialize();
        }

        protected override void Configure() {
            // Register Types
            _container.RegisterSingleton<IAppMeta>(() => new ApplicationMeta(APPNAME));

            CorePackage.RegisterServices(_container);
            ManagerPackage.RegisterServices(_container);

            // Register windows and view models
            _container.Register<IWindowManager, WindowManager>();
            _container.RegisterSingleton<IEventAggregator, EventAggregator>();

            _container.Register<MainMenuViewModel>();

            _container.Verify();
        }

        protected override void OnStartup(object sender, StartupEventArgs e) {
            var settings = _container.GetInstance<ISettingStore>();
            if (settings != null) settings.Load();
            DisplayRootViewFor<MainViewModel>();
        }

        protected override void OnExit(object sender, EventArgs e) {
            var settings = _container.GetInstance<ISettingStore>();
            if (settings != null) settings.Save();
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            //_container.GetAllInstances(service);
            IServiceProvider provider = _container;
            Type collectionType = typeof(IEnumerable<>).MakeGenericType(service);
            var services = (IEnumerable<object>)provider.GetService(collectionType);
            return services ?? Enumerable.Empty<object>();
        }

        protected override object GetInstance(Type service, string key) {
            return _container.GetInstance(service);
        }

        protected override IEnumerable<Assembly> SelectAssemblies() {
            return new[] {
                Assembly.GetExecutingAssembly()
            };
        }

        protected override void BuildUp(object instance) {
            var registration = _container.GetRegistration(instance.GetType(), true);
            registration.Registration.InitializeInstance(instance);
        }
    }
}
