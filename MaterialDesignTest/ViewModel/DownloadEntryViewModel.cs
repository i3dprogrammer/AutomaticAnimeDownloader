using MaterialDesignTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignTest.ViewModel
{
    class DownloadEntryViewModel : ViewModelBase
    {
        public DownloadEntryViewModel(DownloadEntry downloadEntry)
        {
            this._downloadEntry = downloadEntry;
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
            get
            {
                return _downloadEntry.Status;
            }
            set
            {
                _downloadEntry.Status = value;
                OnPropertyChanged("Status");
            }
        }
        public int Progress
        {
            get
            {
                return _downloadEntry.Progress;
            }
            set
            {
                _downloadEntry.Progress = value;
                OnPropertyChanged("Progress");
            }
        }
        public string DSpeed
        {
            get
            {
                return _downloadEntry.DSpeed;
            }
            set
            {
                _downloadEntry.DSpeed = value;
                OnPropertyChanged("DSpeed");
            }
        }
        public string USpeed
        {
            get
            {
                return _downloadEntry.USpeed;
            }
            set
            {
                _downloadEntry.USpeed = value;
                OnPropertyChanged("USpeed");
            }
        }

    }
}
