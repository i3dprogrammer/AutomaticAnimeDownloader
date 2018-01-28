using MaterialDesignTest.Models;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace MaterialDesignTest.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private Settings settings { get; set; }
        private MALParser.Lists.AnimeListManager animeListManager { get; set; }
        private string filterText = "";
        private string manualTitle = "";

        public SettingsViewModel(Settings settings)
        {
            this.settings = settings;

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
            });

            RemoveEntryFromDownloadList = new DelegateCommand(o =>
            {
                var temp = new List<AnimeViewModel>();
                foreach (AnimeViewModel item in o as IList)
                    temp.Add(item);

                foreach (var item in temp)
                    DownloadList.Remove(item);
            });

            AddManualTitleCommand = new DelegateCommand(o =>
            {
                DownloadList.Add(new AnimeViewModel(new Anime() { Title = ManualTitle, Quality = this.Quality }));
                ManualTitle = "";
            });

            this.animeListManager = new MALParser.Lists.AnimeListManager();

            RefreshAiringAnimes();
        }

        private async void RefreshAiringAnimes()
        {
            Loading = Visibility.Visible;
            MainAnimeList.Clear();
            (await animeListManager.GetAiringScheduleAsync(MALParser.ScheduleDay.Any)).Animes.ForEach(x =>
            {
                var item = new AnimeViewModel(new Anime() { Title = x.Title });
                MainAnimeList.Add(item);
            });
            FilterAnimeList();
            Loading = Visibility.Hidden;
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

        public ICommand DownloadsPathSelectCommand { get; private set; }
        public ICommand TorrentsPathSelectCommand { get; private set; }
        public ICommand ConfigPathSelectCommand { get; private set; }
        public ICommand RefreshAiringAnimesCommand { get; private set; }
        public ICommand AddAnimeToDownloadListCommand { get; set; }
        public ICommand RemoveEntryFromDownloadList { get; set; }
        public ICommand AddManualTitleCommand { get; set; }

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
        public ObservableCollection<string> Qualities { get; set; } = new ObservableCollection<string>() { "1080p", "720p", "480p" };
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
