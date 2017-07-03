using Caliburn.Micro;
using Safir.Core.Application;
using Safir.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Safir.ViewModels
{
    public class MainViewModel : Screen
    {
        private readonly ISettingStore _settings;

        public MainViewModel(
            IAppMeta appMeta,
            ISettingStore settings) {
            AppName = appMeta.AppName;
            _settings = settings;
        }

        public string AppName { get; set; }

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
