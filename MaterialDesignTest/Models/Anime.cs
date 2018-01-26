using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MALParser.Dto.ListModels;
using System.Collections.ObjectModel;

namespace MaterialDesignTest.Models
{
    public class Anime : ViewModelBase
    {
        public bool Download { get; set; }
        public string Title { get; set; }
    }
}
