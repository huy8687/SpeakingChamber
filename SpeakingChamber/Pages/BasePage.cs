using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SpeakingChamber.ViewModel;

namespace SpeakingChamber.Pages
{
    public abstract class BasePage : Page
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
            Loaded += BasePage_Loaded;
            Unloaded += BasePage_Unloaded;
        }

        private void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.Appearing();
        }

        private void BasePage_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel.Disappearing();
        }
    }
}
