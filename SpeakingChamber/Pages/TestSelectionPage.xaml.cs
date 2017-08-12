using SpeakingChamber.ViewModel;

namespace SpeakingChamber.Pages
{
    /// <summary>
    /// Interaction logic for TestSelectionPage.xaml
    /// </summary>
    public partial class TestSelectionPage : BasePage
    {
        public TestSelectionPage()
        {
            InitializeComponent();
            ViewModel = new TestSelectionViewModel();
        }
    }
}
