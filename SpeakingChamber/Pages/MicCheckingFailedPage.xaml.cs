using SpeakingChamber.ViewModel;

namespace SpeakingChamber.Pages
{
    /// <summary>
    /// Interaction logic for MicCheckingFailedPage.xaml
    /// </summary>
    public partial class MicCheckingFailedPage : BasePage
    {
        public MicCheckingFailedPage()
        {
            InitializeComponent();
            ViewModel = new MicCheckingFailedViewModel();
        }
    }
}
