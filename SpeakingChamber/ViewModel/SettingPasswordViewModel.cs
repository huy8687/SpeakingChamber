using SpeakingChamber.Extension;
using SpeakingChamber.Model;
using SpeakingChamber.Pages;
using System.Windows.Input;

namespace SpeakingChamber.ViewModel
{
    public class SettingPasswordViewModel : BaseViewModel
    {
        public string Password { get; set; }

        public ICommand CmdOK => new Command(() =>
        {
            if (Password == "Excelschool@123!")
            {
                Navigation.Navigate(new SettingUpdatingPage());
            }
            else
            {
                Navigation.PopToRoot();
            }
        });
    }
}
