using SpeakingChamber.ViewModel;

namespace SpeakingChamber.Pages
{
    /// <summary>
    /// Interaction logic for SettingPasswordPage.xaml
    /// </summary>
    public partial class SettingPasswordPage : BasePage
    {
        public SettingPasswordPage()
        {
            InitializeComponent();
            ViewModel = new SettingPasswordViewModel();
        }
    }
}
