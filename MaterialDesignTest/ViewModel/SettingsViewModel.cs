using MaterialDesignTest.Models;
using Microsoft.Win32;
using System;
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
        private int i = 0;
        private Settings settings { get; set; }
        private MALParser.Lists.AnimeListManager animeListManager { get; set; }

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
                //AnimeList.Add(new AnimeViewModel(new Anime() { Download = i % 2 == 0, Title = $"Anime {i++}" }));
                RefreshAiringAnimes();
            });

            this.animeListManager = new MALParser.Lists.AnimeListManager();

            RefreshAiringAnimes();
        }

        private async void RefreshAiringAnimes()
        {
            Loading = Visibility.Visible;
            List<string> selectedItems = new List<string>();
            AnimeList.ToList().ForEach(x =>
            {
                if (x.Download)
                    selectedItems.Add(x.Title);
            });
            AnimeList.Clear();
            (await animeListManager.GetAiringScheduleAsync(MALParser.ScheduleDay.Any)).Animes.ForEach(x =>
            {
                AnimeList.Add(new AnimeViewModel(new Anime() { Download = selectedItems.Contains(x.Title), Title = x.Title }));
            });
            Loading = Visibility.Hidden;
        }

        public ICommand DownloadsPathSelectCommand { get; private set; }
        public ICommand TorrentsPathSelectCommand { get; private set; }
        public ICommand ConfigPathSelectCommand { get; private set; }
        public ICommand RefreshAiringAnimesCommand { get; private set; }

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
        public ObservableCollection<AnimeViewModel> AnimeList
        {
            get
            {
                return settings.AnimeList;
            }
        }
    }
}
