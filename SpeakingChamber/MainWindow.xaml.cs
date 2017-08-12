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
        }
    }
}
