using MaterialDesignTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignTest.Models
{
    class Downloader
    {
        public string TotalDownloaded { get; set; }
        public string TotalUploaded { get; set; }

        public ObservableCollection<DownloadEntryViewModel> DownloadList { get; set; }

        public Downloader()
        {
            DownloadList = new ObservableCollection<DownloadEntryViewModel>();
        }
    }
}
