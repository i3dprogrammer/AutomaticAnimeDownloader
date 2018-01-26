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

        public ObservableCollection<AnimeViewModel> AnimeList { get; set; }

        public Settings()
        {
            AnimeList = new ObservableCollection<AnimeViewModel>();
            Loading = Visibility.Hidden;
        }
    }
}
