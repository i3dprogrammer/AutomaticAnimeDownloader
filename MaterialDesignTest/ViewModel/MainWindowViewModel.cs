using MaterialDesignTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MaterialDesignTest.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static DownloadManager.Manager _downloadManager;

        private int timerCheckInterval = 30 * 60 * 1000;
        private SettingsViewModel _settingsViewModel;
        private AboutViewModel _aboutViewModel;
        private DownloaderViewModel _downloaderViewModel;
        private DownloadManager.NyaaWrapper _nyaaWrapper;
        private Timer checkForNewEpisodes_timer;
        private Timer startTheDownloader_timer;
        private IniFile _configFile;

        public ICommand closeApplication { get; private set; }

        public MainWindowViewModel()
        {
            _configFile = new IniFile();
            // View model data.
            _settingsViewModel = new SettingsViewModel(new Settings(), _configFile);
            _aboutViewModel = new AboutViewModel(new Todo());
            _downloaderViewModel = new DownloaderViewModel(new Downloader() { Paused = false });

            CurrentViewModel = _settingsViewModel;

            // Initializing the downloader.

            _downloadManager = new DownloadManager.Manager();
            _downloadManager.SetConfigPath();
            _downloadManager.SetDownloadPath();
            _downloadManager.SetTorrentsPath();
            _downloadManager.InitializeEngine();

            // Instantiating Nyaa wrapper.
            _nyaaWrapper = new DownloadManager.NyaaWrapper();

            // To check for the download state change.
            _downloaderViewModel.PropertyChanged += _downloaderViewModel_PropertyChanged;
            _settingsViewModel.PropertyChanged += _settingsViewModel_PropertyChanged;

            // Close application command
            closeApplication = new DelegateCommand(o =>
            {
                _settingsViewModel.SaveSettings();
                if (_settingsViewModel.MinimizeOnExit)
                    CurrentWindowState = WindowState.Minimized;
                else
                {
                    try
                    {
                        _downloadManager.ShutdownEngine();
                        Environment.Exit(0);
                    } catch
                    {
                        Environment.Exit(0);
                    }
                }
            });

            // The periodically timer check for new episodes.
            checkForNewEpisodes_timer = new Timer(DownloadAllTorrents, null, timerCheckInterval, timerCheckInterval);

            foreach (var file in System.IO.Directory.GetFiles("Downloads"))
            {
                if (file.EndsWith(".torrent"))
                {
                    var item = new DownloadEntry()
                    {
                        TorrentManager = _downloadManager.AddTorrentFile(file),
                    };
                    _downloaderViewModel.DownloadList.Add(new DownloadEntryViewModel(item));
                    item.Title = item.TorrentManager.Torrent.Files[0].Path;
                    item.Size = "0";
                }
            }
        }

        private void _settingsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _settingsViewModel.SaveSettings();
        }

        private void _downloaderViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // If the downloader state changed, change all the manager states.
            if (e.PropertyName == "Paused")
            {
                if (_downloaderViewModel.Paused == false)
                    _downloadManager.StartAll();
                else
                    _downloadManager.StopAll();
            }
        }

        public async void DownloadAllTorrents(object state)
        {
            var items = (await _nyaaWrapper.ParseProvidersSubsAsync());
            try
            {
                items.ToList().ForEach(x =>
                {
                    MessageBox.Show(x.Title);
                    // If the item doesn't already exist in the downloader queue.
                    if (!_downloaderViewModel.DownloadList.ToList().Exists(z => z.Title == $"{x.Title} - {x.Episode} - [{x.Quality}]"))
                    {
                        // If the episode exists in our "should download list"
                        if (_settingsViewModel.DownloadList.ToList().Exists(y => y.Title == x.Title && y.Quality == x.Quality))
                        {
                            var item = new DownloadEntry()
                            {
                                Title = $"{x.Title} - {x.Episode} - [{x.Quality}]",
                                Size = x.TorrentSize,
                                TorrentManager = _downloadManager.AddTorrentUrl(x.TorrentURL),
                            };
                            _downloaderViewModel.DownloadList.Add(new DownloadEntryViewModel(item));

                            // If the downloader is started, start downloading the episode.
                            if (!_downloaderViewModel.Paused)
                                item.TorrentManager.Start();
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region ViewModelData

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


        #endregion
    }
}
