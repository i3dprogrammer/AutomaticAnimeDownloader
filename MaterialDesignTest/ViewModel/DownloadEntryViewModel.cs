using MaterialDesignTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace MaterialDesignTest.ViewModel
{
    class DownloadEntryViewModel : ViewModelBase
    {
        private Timer timer;

        public DownloadEntryViewModel(DownloadEntry downloadEntry)
        {
            this._downloadEntry = downloadEntry;

            timer = new Timer(1000);
            timer.Elapsed += (o, k) => {
                OnPropertyChanged("Status");
                OnPropertyChanged("Progress");
                OnPropertyChanged("DSpeed");
                OnPropertyChanged("USpeed");
            };
            timer.Start();
        }

        private DownloadEntry _downloadEntry { get; set; }

        public string Title
        {
            get
            {
                return _downloadEntry.Title;
            }
            set
            {
                _downloadEntry.Title = value;
                OnPropertyChanged("Title");
            }
        }
        public string Size
        {
            get
            {
                return _downloadEntry.Size;
            }
            set
            {
                _downloadEntry.Size = value;
                OnPropertyChanged("Size");
            }
        }
        public string Status
        {
            get { return _downloadEntry.TorrentManager.State.ToString(); }
        }
        public int Progress
        {
            get { return (int)_downloadEntry.TorrentManager.Progress; }
            set { }
        }
        public string DSpeed
        {
            get { return $"{_downloadEntry.TorrentManager.Monitor.DownloadSpeed / 1024} KB/s"; }
        }
        public string USpeed
        {
            get { return $"{_downloadEntry.TorrentManager.Monitor.UploadSpeed / 1024} KB/s"; }
        }
    }
}
