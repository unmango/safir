using Caliburn.Micro;
using Moq;
using Safir.Core.Application;
using Safir.Core.Settings;
using Safir.ViewModels;
using Xunit;

namespace Safir.Tests.ViewModels
{
    public class MainViewModelTest
    {
        public Mock<IAppMeta> appMeta;
        public Mock<ISettingStore> settings;
        public Mock<IEventAggregator> eventAggregator;
        public Mock<MainMenuViewModel> mainMenuViewModel;

        public MainViewModel viewModel;

        public MainViewModelTest() {
            viewModel = new MainViewModel(
                appMeta.Object,
                settings.Object,
                eventAggregator.Object,
                mainMenuViewModel.Object);
        }
    }
}
