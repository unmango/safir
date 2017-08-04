// <copyright file="MainMenuViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.ViewModels
{
    using Caliburn.Micro;
    using Core.Settings;

    public class MainMenuViewModel : Conductor<object>
    {
        private readonly ISettingStore _settings;
        private readonly IWindowManager _windowManager;

        // private readonly PreferencesViewModel _preferences;

        public MainMenuViewModel(
            ISettingStore settings,
            IWindowManager manager) {
            // PreferencesViewModel preferences

            _settings = settings;
            _windowManager = manager;
            // _preferences = preferences;
        }

        public void OpenPreferences() {
            // ActivateItem(_preferences);
            // _windowManager.ShowDialog(_preferences);
        }
    }
}
