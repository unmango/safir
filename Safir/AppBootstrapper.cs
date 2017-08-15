// <copyright file="AppBootstrapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using Caliburn.Micro;
    using Core;
    using Core.Application;
    using Core.Settings;
    using Data;
    using Logging;
    using Manager;
    using SimpleInjector;
    using ViewModels;

    internal class AppBootstrapper : BootstrapperBase
    {
        private const string APPNAME = "Safir";
        private static readonly Container Container = new Container();

        public AppBootstrapper() {
            LogManager.GetLog = type => new Log4NetLogger(type);
            Initialize();
        }

        protected override void Configure() {
            // Register Types
            Container.RegisterSingleton<IAppMeta>(() => new ApplicationMeta(APPNAME));

            CorePackage.RegisterServices(Container);
            DataPackage.RegisterServices(Container);
            ManagerPackage.RegisterServices(Container);

            // Register windows and view models
            // Ok maybe not viewmodels? Haven't registered any yet and things seem to work just fine
            Container.Register<IWindowManager, WindowManager>();
            Container.RegisterSingleton<IEventAggregator, EventAggregator>();

            Container.Verify();
        }

        protected override void OnStartup(object sender, StartupEventArgs e) {
            var settings = Container.GetInstance<ISettingStore>();
            settings?.Load();
            DisplayRootViewFor<MainViewModel>();
        }

        protected override void OnExit(object sender, EventArgs e) {
            var settings = Container.GetInstance<ISettingStore>();
            settings?.Save();
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            // _container.GetAllInstances(service);
            IServiceProvider provider = Container;
            var collectionType = typeof(IEnumerable<>).MakeGenericType(service);
            var services = (IEnumerable<object>)provider.GetService(collectionType);
            return services ?? Enumerable.Empty<object>();
        }

        protected override object GetInstance(Type service, string key) {
            return Container.GetInstance(service);
        }

        protected override IEnumerable<Assembly> SelectAssemblies() {
            return new[] {
                Assembly.GetExecutingAssembly()
            };
        }

        protected override void BuildUp(object instance) {
            var registration = Container.GetRegistration(instance.GetType(), true);
            registration.Registration.InitializeInstance(instance);
        }
    }
}
