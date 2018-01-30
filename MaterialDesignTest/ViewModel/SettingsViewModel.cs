using MaterialDesignTest.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

// Ensures that the program is working correctly
// Fix the bugs
// Actually implement downloads folder & download throtle & time management.
namespace MaterialDesignTest.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private Settings settings { get; set; }
        private MALParser.Lists.AnimeListManager animeListManager { get; set; }
        private string filterText = "";
        private string manualTitle = "";
        private string userListName = "";
        private bool dialogHostOpen = false;
        private IniFile configFile;


        public SettingsViewModel(Settings settings, IniFile configFile)
        {
            this.settings = settings;
            this.configFile = configFile;

            LoadSettings();

            DownloadsPathSelectCommand = new DelegateCommand(o =>
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                    DownloadsPath = dialog.SelectedPath;
            });

            TorrentsPathSelectCommand = new DelegateCommand(o =>
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                    TorrentsPath = dialog.SelectedPath;
            });

            ConfigPathSelectCommand = new DelegateCommand(o =>
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                    ConfigPath = dialog.SelectedPath;
            });

            RefreshAiringAnimesCommand = new DelegateCommand(o =>
            {
                RefreshAiringAnimes();
            });

            AddAnimeToDownloadListCommand = new DelegateCommand(o =>
            {
                foreach (AnimeViewModel item in ((IList)o))
                {
                    if (!DownloadList.ToList().Exists(x => x.Title == item.Title && x.Quality == this.Quality))
                        DownloadList.Add(new AnimeViewModel(new Anime() { Title = item.Title, Quality = this.Quality }));
                }

                SaveSettings();
            });

            RemoveEntryFromDownloadList = new DelegateCommand(o =>
            {
                var temp = new List<AnimeViewModel>();
                foreach (AnimeViewModel item in o as IList)
                    temp.Add(item);

                foreach (var item in temp)
                    DownloadList.Remove(item);

                SaveSettings();
            });

            AddManualTitleCommand = new DelegateCommand(o =>
            {
                DownloadList.Add(new AnimeViewModel(new Anime() { Title = ManualTitle, Quality = this.Quality }));
                ManualTitle = "";
                SaveSettings();
            });

            Fetch_MALAPI_AnimeList = new DelegateCommand(o =>
            {
                DialogHostOpen = false;
                FetchMALAPIAnimeList();
            });

            this.animeListManager = new MALParser.Lists.AnimeListManager();

            RefreshAiringAnimes();

        }

        private async void RefreshAiringAnimes()
        {
            Loading = Visibility.Visible;
            MainAnimeList.Clear();
            AnimeList.Clear();
            (await animeListManager.GetAiringScheduleAsync(MALParser.ScheduleDay.Any)).Animes.ForEach(x =>
            {
                var item = new AnimeViewModel(new Anime() { Title = x.Title });
                MainAnimeList.Add(item);
            });
            FilterAnimeList();
            Loading = Visibility.Hidden;
        }
        private async void FetchMALAPIAnimeList()
        {
            try
            {
                MALAPI.API api = new MALAPI.API();
                var animes = await api.UsersController.GetUserAnimeListAsync(UserListName);
                animes.Animes.ForEach(x =>
                {
                    if (x.MyStatus == MALAPI.AnimeListStatus.Watching)
                    {
                        if (x.SeriesStatus == "1")
                            DownloadList.Add(new AnimeViewModel(new Anime() { Title = x.SeriesTitle, Quality = this.Quality }));
                    }
                });
            }
            catch (Exception ex)
            {
                TextBlock block = new TextBlock();
                block.Margin = new Thickness(20);
                block.Text = ex.Message;
                await DialogHost.Show(ex.Message);
            }
        }
        private void FilterAnimeList()
        {
            AnimeList.Clear();
            MainAnimeList.ToList().ForEach(x =>
            {
                if (x.Title.ToLower().Contains(FilterText.ToLower()))
                    AnimeList.Add(x);
            });
        }

        public void SaveSettings()
        {
            try
            {
                configFile.Write("UserListName", UserListName, "Settings");
                configFile.Write("DownloadsPath", DownloadsPath, "Settings");
                configFile.Write("TorrentsPath", TorrentsPath, "Settings");
                configFile.Write("ConfigPath", ConfigPath, "Settings");
                configFile.Write("Quality", Quality, "Settings");
                configFile.Write("MinimizeOnExit", MinimizeOnExit.ToString(), "Settings");
                configFile.Write("MaxDownloadSpeed", MaxDownloadSpeed, "Settings");
                configFile.Write("DownloadList", DownloadList.Select(x => $"{x.Title}~{x.Quality}").Aggregate((x, y) => $"{x},{y}"), "Settings");
            }
            catch { }
        }

        public void LoadSettings()
        {
            try
            {
                UserListName = configFile.Read("UserListName", "Settings");
                DownloadsPath = configFile.Read("DownloadsPath", "Settings");
                TorrentsPath = configFile.Read("TorrentsPath", "Settings");
                ConfigPath = configFile.Read("ConfigPath", "Settings");
                Quality = configFile.Read("Quality", "Settings");
                MinimizeOnExit = configFile.Read("MinimizeOnExit", "Settings") == "true";
                MaxDownloadSpeed = configFile.Read("MaxDownloadSpeed", "Settings");
                configFile.Read("DownloadList", "Settings").Split(',').ToList().ForEach(x =>
                {
                    DownloadList.Add(new AnimeViewModel(new Anime() { Title = x.Split('~')[0], Quality = x.Split('~')[1] }));
                });
            }
            catch { }
        }

        public ICommand DownloadsPathSelectCommand { get; private set; }
        public ICommand TorrentsPathSelectCommand { get; private set; }
        public ICommand ConfigPathSelectCommand { get; private set; }
        public ICommand RefreshAiringAnimesCommand { get; private set; }
        public ICommand Fetch_MALAPI_AnimeList { get; private set; }
        public ICommand AddAnimeToDownloadListCommand { get; private set; }
        public ICommand RemoveEntryFromDownloadList { get; private set; }
        public ICommand AddManualTitleCommand { get; private set; }

        public bool DialogHostOpen
        {
            get { return dialogHostOpen; }
            set
            {
                dialogHostOpen = value;
                OnPropertyChanged("DialogHostOpen");
            }
        }
        public string UserListName
        {
            get
            {
                return userListName;
            }
            set
            {
                userListName = value;
                OnPropertyChanged("UserListName");
            }
        }
        public string ManualTitle
        {
            get { return manualTitle; }
            set
            {
                manualTitle = value;
                OnPropertyChanged("ManualTitle");
            }
        }
        public string FilterText
        {
            get { return filterText; }
            set
            {
                filterText = value;
                OnPropertyChanged("FilterText");
                FilterAnimeList();
            }
        }
        public string DownloadsPath
        {
            get
            {
                return settings.DownloadsPath;
            }
            set
            {
                settings.DownloadsPath = value;
                OnPropertyChanged("DownloadsPath");
            }
        }
        public string TorrentsPath
        {
            get
            {
                return settings.TorrentsPath;
            }
            set
            {
                settings.TorrentsPath = value;
                OnPropertyChanged("TorrentsPath");
            }
        }
        public string ConfigPath
        {
            get
            {
                return settings.ConfigPath;
            }
            set
            {
                settings.ConfigPath = value;
                OnPropertyChanged("ConfigPath");
            }
        }
        public Visibility Loading
        {
            get
            {
                return settings.Loading;
            }
            set
            {
                settings.Loading = value;
                OnPropertyChanged("Loading");
            }
        }
        public bool RefreshButtonEnabled
        {
            get
            {
                return Loading == Visibility.Hidden;
            }
        }
        public string Quality
        {
            get
            {
                return settings.Quality;
            }
            set
            {
                settings.Quality = value;
                OnPropertyChanged("Quality");
            }
        }
        public bool MinimizeOnExit
        {
            get
            {
                return settings.MinimizeOnExit;
            }
            set
            {
                settings.MinimizeOnExit = value;
                OnPropertyChanged("MinimizeOnExit");
            }
        }
        public string MaxDownloadSpeed
        {
            get
            {
                return settings.MaxDownloadSpeed;
            }
            set
            {
                settings.MaxDownloadSpeed = value;
                OnPropertyChanged("MaxDownloadSpeed");
            }
        }

        public ObservableCollection<string> Qualities { get; set; } = new ObservableCollection<string>() { "1080p", "720p", "480p" };
        public ObservableCollection<string> MaxDownloadSpeeds { get; set; } = new ObservableCollection<string>() {
            "0 - Unlimited",
            "50 KB/s",
            "100 KB/s",
            "200 KB/s",
            "300 KB/s",
            "400 KB/s",
            "500 KB/s",
            "600 KB/s",
            "700 KB/s",
            "800 KB/s",
            "900 KB/s",
            "1 MB/s",
        };

        public List<AnimeViewModel> MainAnimeList
        {
            get
            {
                return settings.MainAnimeList;
            }
        }
        public ObservableCollection<AnimeViewModel> AnimeList { get; set; } = new ObservableCollection<AnimeViewModel>();
        public ObservableCollection<AnimeViewModel> DownloadList
        {
            get
            {
                return settings.DownloadList;
            }
        }
    }
}
