using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Safir.ViewModels
{
    using Core.Settings;

    public class MainMenuViewModel : PropertyChangedBase
    {
        private readonly ISettingStore _settings;
        private readonly IWindowManager _windowManager;

        private readonly PreferencesViewModel _preferences;

        public MainMenuViewModel(
            ISettingStore settings,
            IWindowManager manager,
            PreferencesViewModel preferences)
        {
            _settings = settings;
            _windowManager = manager;
            _preferences = preferences;
        }

        public void OpenPreferences() =>
            _windowManager.ShowDialog(_preferences);
    }
}
