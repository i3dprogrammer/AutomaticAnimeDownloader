using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignTest.Models
{
    class DownloadEntry
    {
        public string Title { get; set; }
        public string Size { get; set; }
        public int Progress { get; set; }
        public string Status { get; set; }
        public string DSpeed { get; set; }
        public string USpeed{ get; set; }
    }
}
