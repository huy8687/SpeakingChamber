using SpeakingChamber.ViewModel;

namespace SpeakingChamber.Pages
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : BasePage
    {
        public TestPage()
        {
            InitializeComponent();
            ViewModel = new TestViewModel(VideoView);
        }
    }
}
