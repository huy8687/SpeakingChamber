using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SpeakingChamber.ViewModel;

namespace SpeakingChamber.Pages
{
    /// <summary>
    /// Interaction logic for OpeningPage.xaml
    /// </summary>
    public partial class OpeningPage
    {
        public OpeningPage()
        {
            InitializeComponent();
            ViewModel = new OpeningViewModel();
        }
    }
}
