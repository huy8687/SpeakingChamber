using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SpeakingChamber.ViewModel;

namespace SpeakingChamber.Pages
{
    public class BasePage : Page
    {
        private BaseViewModel _viewModel;

        public BaseViewModel ViewModel
        {
            get => _viewModel;
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
