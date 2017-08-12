using SpeakingChamber.ViewModel;

namespace SpeakingChamber.Pages
{
    /// <summary>
    /// Interaction logic for MicCheckingPage.xaml
    /// </summary>
    public partial class MicCheckingPage : BasePage
    {
        public MicCheckingPage()
        {
            InitializeComponent();
            ViewModel = new MicCheckingViewModel();
        }
    }
}
