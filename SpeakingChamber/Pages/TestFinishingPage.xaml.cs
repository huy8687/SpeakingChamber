using SpeakingChamber.ViewModel;

namespace SpeakingChamber.Pages
{
    /// <summary>
    /// Interaction logic for TestFinishingPage.xaml
    /// </summary>
    public partial class TestFinishingPage : BasePage
    {
        public TestFinishingPage()
        {
            InitializeComponent();
            ViewModel = new TestFinishingViewModel();
        }
    }
}
