using SpeakingChamber.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpeakingChamber.Pages
{
    public abstract class BasePage : Page
    {
        private BaseViewModel _viewModel;

        public BaseViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                DataContext = value;
            }
        }

        public BasePage()
        {
        }

    }
}
