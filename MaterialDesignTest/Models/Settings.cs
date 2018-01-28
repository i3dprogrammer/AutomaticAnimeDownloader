using MaterialDesignTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialDesignTest.Models
{
    public class Settings
    {
        public string DownloadsPath { get; set; }
        public string TorrentsPath { get; set; }
        public string ConfigPath { get; set; }
        public Visibility Loading { get; set; }
        public string Quality { get; set; } = "480p";
        public bool MinimizeOnExit { get; set; } = false;

        public List<AnimeViewModel> MainAnimeList { get; set; }
        public ObservableCollection<AnimeViewModel> DownloadList { get; set; }

        public Settings()
        {
            MainAnimeList = new List<AnimeViewModel>();
            DownloadList = new ObservableCollection<AnimeViewModel>() { new AnimeViewModel(new Anime() { Title = "Beatless", Quality = "480p" }) };
            Loading = Visibility.Hidden;
        }
    }
}
