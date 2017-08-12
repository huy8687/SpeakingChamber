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
