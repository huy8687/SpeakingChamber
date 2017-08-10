using SpeakingChamber.Model;
using SpeakingChamber.Pages;
using SpeakingChamber.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            //MainFrame.Content = new OpeningPage();
            MainFrame.NavigationService.Navigate(new OpeningPage());
            BaseViewModel.Navigation = MainFrame.NavigationService;
        }
    }
}
