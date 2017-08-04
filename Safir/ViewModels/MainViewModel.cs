// <copyright file="MainViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.ViewModels
{
    using System.Windows;
    using Caliburn.Micro;
    using Safir.Core.Application;
    using Safir.Core.Settings;

    public class MainViewModel : Screen
    {
        private readonly ISettingStore _settings;

        public MainViewModel(
            IAppMeta appMeta,
            ISettingStore settings,
            MainMenuViewModel mainMenuViewModel) {
            AppName = appMeta.AppName;
            _settings = settings;
            MainMenu = mainMenuViewModel;
        }

        public string AppName { get; set; }

        public MainMenuViewModel MainMenu { get; set; }

        #region Window Settings

        protected override void OnInitialize() {
            var view = GetView() as Window;
            view.Height = _settings.Get<double>("WindowHeight");
            view.Width = _settings.Get<double>("WindowWidth");
            view.Left = _settings.Get<double>("WindowLeft");
            view.Top = _settings.Get<double>("WindowTop");
            view.WindowState = _settings.Get<WindowState>("WindowState");
        }

        protected override void OnDeactivate(bool close) {
            var view = GetView() as Window;
            _settings.Set("WindowHeight", view.Height);
            _settings.Set("WindowWidth", view.Width);
            _settings.Set("WindowLeft", view.Left);
            _settings.Set("WindowTop", view.Top);
            _settings.Set("WindowState", view.WindowState);
        }

        #endregion
    }
}
