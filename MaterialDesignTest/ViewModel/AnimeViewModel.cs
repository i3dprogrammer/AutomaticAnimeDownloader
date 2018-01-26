using MaterialDesignTest.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignTest.ViewModel
{
    public class AnimeViewModel : ViewModelBase
    {
        private Anime anime { get; set; }

        public AnimeViewModel(Anime anime)
        {
            this.anime = anime;
        }


        public string Title
        {
            get
            {
                return anime.Title;
            }
            set
            {
                anime.Title = value;
                OnPropertyChanged("Title");
            }
        }

        public bool Download
        {
            get
            {
                return anime.Download;
            }
            set
            {
                anime.Download = value;
                OnPropertyChanged("Download");
            }
        }
    }
}
