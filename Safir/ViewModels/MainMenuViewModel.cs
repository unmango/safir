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

        public MainMenuViewModel(ISettingStore settings)
        {
            _settings = settings;
        }
    }
}
