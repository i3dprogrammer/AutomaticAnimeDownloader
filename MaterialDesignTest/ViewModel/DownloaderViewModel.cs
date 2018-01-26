using MaterialDesignTest.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace MaterialDesignTest.ViewModel
{
    class DownloaderViewModel : ViewModelBase
    {
        private Downloader _downloader { get; set; }

        public DownloaderViewModel(Downloader downloader)
        {
            this._downloader = downloader;
        }

        public ObservableCollection<DownloadEntryViewModel> DownloadList
        {
            get
            {
                return _downloader.DownloadList;
            }
        }

        public string TotalDownloaded
        {
            get
            {
                return _downloader.TotalDownloaded;
            }
            set
            {
                _downloader.TotalDownloaded = value;
                OnPropertyChanged("TotalDownloaded");
            }
        }
        public string TotalUploaded
        {
            get
            {
                return _downloader.TotalUploaded;
            }
            set
            {
                _downloader.TotalUploaded = value;
                OnPropertyChanged("TotalUploaded");
            }
        }
    }
}
