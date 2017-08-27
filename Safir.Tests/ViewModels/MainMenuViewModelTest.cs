using Caliburn.Micro;
using Moq;
using Safir.Manager;
using Safir.ViewModels;
using Xunit;

namespace Safir.Tests.ViewModels
{
    public class MainMenuViewModelTest
    {
        public Mock<IEventAggregator> eventAggregator = new Mock<IEventAggregator>();
        public Mock<IWindowManager> windowManager = new Mock<IWindowManager>();
        public Mock<MusicManager> musicManager = new Mock<MusicManager>(null);
        public Mock<PreferencesViewModel> preferencesViewModel = new Mock<PreferencesViewModel>(null);

        public readonly MainMenuViewModel viewModel;

        public MainMenuViewModelTest() {
            viewModel = new MainMenuViewModel(
                eventAggregator.Object,
                windowManager.Object,
                musicManager.Object,
                preferencesViewModel.Object);
        }

        [Fact]
        public void AddFileTest() {
            viewModel.AddFile();
            musicManager.Verify(x => x.AddFile(""));
        }
    }
}
