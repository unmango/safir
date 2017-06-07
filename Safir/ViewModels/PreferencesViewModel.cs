using Caliburn.Micro;

namespace Safir.ViewModels
{
    using Core.Settings;
    using Logging;

    public class PreferencesViewModel : Conductor<object>
    {
        private readonly ILogger _log;

        private ISettingStore _settings;

        public PreferencesViewModel(
            ISettingStore settings) {
            _log = (ILogger)LogManager.GetLog(typeof(PreferencesViewModel));
            _settings = settings;
        }


    }
}
