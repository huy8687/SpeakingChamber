using SpeakingChamber.ViewModel;

namespace SpeakingChamber.Pages
{
    /// <summary>
    /// Interaction logic for StartExamPage.xaml
    /// </summary>
    public partial class StartExamPage : BasePage
    {
        public StartExamPage()
        {
            InitializeComponent();
            ViewModel = new StartExamViewModel();
        }
    }
}
