using MaterialDesignTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignTest.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            CurrentViewModel = new SettingsViewModel(new Models.Settings());
            //CurrentViewModel = new AnimeListViewModel(new Models.AnimeList());
            //CurrentViewModel = new AboutViewModel(new Todo());

        }

        // ViewModel that is currently bound to the ContentControl
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                this.OnPropertyChanged("CurrentViewModel");
            }
        }

    }
}
