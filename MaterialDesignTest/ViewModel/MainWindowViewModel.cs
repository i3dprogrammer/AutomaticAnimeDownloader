using MaterialDesignTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MaterialDesignTest.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static DownloadManager.Manager _downloadManager;
        private SettingsViewModel _settingsViewModel;
        private AboutViewModel _aboutViewModel;
        private DownloaderViewModel _downloaderViewModel;
        private DownloadManager.NyaaWrapper _nyaaWrapper;
        private HttpClient _client;

        public ICommand closeApplication { get; private set; }

        public MainWindowViewModel()
        {
            _downloadManager = new DownloadManager.Manager();
            _downloadManager.SetConfigPath();
            _downloadManager.SetDownloadPath();
            _downloadManager.SetTorrentsPath();
            _downloadManager.InitializeEngine();

            _nyaaWrapper = new DownloadManager.NyaaWrapper();
            _client = new HttpClient();

            _settingsViewModel = new SettingsViewModel(new Settings());
            _aboutViewModel = new AboutViewModel(new Todo());
            _downloaderViewModel = new DownloaderViewModel(new Downloader() { Paused = false });

            _downloaderViewModel.PropertyChanged += _settingsViewModel_PropertyChanged;

            CurrentViewModel = _settingsViewModel;

            closeApplication = new DelegateCommand(o =>
            {
                if (_settingsViewModel.MinimizeOnExit)
                    CurrentWindowState = WindowState.Minimized;
                else
                {
                    _downloadManager.Shutdown();
                    Environment.Exit(0);
                }
            });
        }

        private void _settingsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Paused")
            {
                if (_downloaderViewModel.Paused == false)
                {
                    foreach (var manager in _downloadManager.m_torrents)
                        manager.Start();
                }
                else
                {
                    foreach (var manager in _downloadManager.m_torrents)
                        manager.Stop();
                }
            }
        }

        public async void DownloadAllTorrents()
        {
            var items = (await _nyaaWrapper.ParseHorribleSubsAsync());
            try
            {
                items.ToList().ForEach(x =>
                {
                    if (_settingsViewModel.DownloadList.ToList().Exists(y => y.Title == x.Title && y.Quality == x.Quality))
                    {
                        var item = new DownloadEntry()
                        {
                            Title = $"{x.Title} - {x.Episode} - [{x.Quality}]",
                            Size = x.TorrentSize,
                            TorrentManager = _downloadManager.AddTorrentUrl(x.TorrentURL),
                        };
                        _downloaderViewModel.DownloadList.Add(new DownloadEntryViewModel(item));
                        if (!_downloaderViewModel.Paused)
                            item.TorrentManager.Start();

                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ViewModel that is currently bound to the ContentControl
        private ViewModelBase _currentViewModel;
        private WindowState _currentWindowState;

        public WindowState CurrentWindowState
        {
            get { return _currentWindowState; }
            set
            {
                _currentWindowState = value;
                OnPropertyChanged("CurrentWindowState");
            }
        }
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
