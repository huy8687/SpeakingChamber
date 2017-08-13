using SpeakingChamber.Model;
using SpeakingChamber.Pages;
using SpeakingChamber.ViewModel;
using System.Windows;

namespace SpeakingChamber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataMaster.LoadData();
            MainFrame.Content = new OpeningPage();
            //MainFrame.NavigationService.Navigate(new OpeningPage());
            BaseViewModel.Navigation = MainFrame;
            MainFrame.Navigated += MainFrame_Navigated;
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (MainFrame.Content is OpeningPage || MainFrame.Content is UserLoginPage)
            {
                BtnSetting.Visibility = Visibility.Visible;
            }
            else
            {
                BtnSetting.Visibility = Visibility.Hidden;
            }

        }

        private void BtnMiniMize_Clicked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSetting_Clicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SettingPasswordPage());
        }
    }
}
