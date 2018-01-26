using MaterialDesignTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignTest.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private SettingsViewModel _settingsViewModel;
        private AboutViewModel _aboutViewModel;
        private DownloaderViewModel _downloaderViewModel;

        public MainWindowViewModel()
        {
            _settingsViewModel = new SettingsViewModel(new Settings());
            _aboutViewModel = new AboutViewModel(new Todo());
            _downloaderViewModel = new DownloaderViewModel(new Downloader());

            CurrentViewModel = _settingsViewModel;

        }

        // ViewModel that is currently bound to the ContentControl
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                this.OnPropertyChanged("CurrentViewModel");
            }
        }

        public void ShowSettingsView()
        {
            CurrentViewModel = _settingsViewModel;
        }
        public void ShowAboutView()
        {
            CurrentViewModel = _aboutViewModel;
        }
        public void ShowDownloaderView()
        {
            CurrentViewModel = _downloaderViewModel;
        }

    }
}
