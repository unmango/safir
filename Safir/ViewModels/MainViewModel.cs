﻿// <copyright file="MainViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using Caliburn.Micro;
    using Core.Application;
    using Core.Settings;
    using Events;
    using static System.Double;
    using static System.Windows.SystemParameters;

    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class MainViewModel :
        Screen,
        IHandle<CloseMainWindowEvent>
    {
        private readonly ISettingStore _settings;

        public MainViewModel(
            IAppMeta appMeta,
            ISettingStore settings,
            IEventAggregator eventAggregator,
            MainMenuViewModel mainMenu) {
            AppName = appMeta.AppName;
            _settings = settings;
            eventAggregator.Subscribe(this);
            MainMenu = mainMenu;
        }

        public string AppName { get; set; }

        public MainMenuViewModel MainMenu { get; }

        public void Handle(CloseMainWindowEvent message) => TryClose();

        #region Window Settings

        protected override void OnInitialize() {
            if (!(GetView() is Window view)) return;
            var height = _settings.Get<double>("WindowHeight");
            view.Height = !IsNaN(height) ? height : FullPrimaryScreenHeight;
            var width = _settings.Get<double>("WindowWidth");
            view.Width = !IsNaN(width) ? width : FullPrimaryScreenWidth;
            view.Left = _settings.Get<double>("WindowLeft");
            view.Top = _settings.Get<double>("WindowTop");
            view.WindowState = _settings.Get<WindowState>("WindowState");
        }

        protected override void OnDeactivate(bool close) {
            if (!(GetView() is Window view)) return;
            _settings.Set("WindowHeight", view.Height);
            _settings.Set("WindowWidth", view.Width);
            _settings.Set("WindowLeft", view.Left);
            _settings.Set("WindowTop", view.Top);
            _settings.Set("WindowState", view.WindowState);
        }

        #endregion
    }
}
