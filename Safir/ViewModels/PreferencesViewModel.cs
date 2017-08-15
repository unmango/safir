namespace Safir.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Caliburn.Micro;
    using Core.Settings;

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PreferencesViewModel : Screen
    {
        private readonly ISettingStore _settings;

        public PreferencesViewModel(
            ISettingStore settings) {
            _settings = settings;
        }

        public void Ok() {
            _settings.Save();
            TryClose();
        }

        public void Apply() {
            _settings.Save();
        }

        public void Cancel() {
            TryClose();
        }
    }
}
