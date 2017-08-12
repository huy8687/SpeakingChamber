using SpeakingChamber.Extension;
using SpeakingChamber.Model;
using System.Windows.Input;

namespace SpeakingChamber.ViewModel
{
    class MicCheckingFailedViewModel : BaseViewModel
    {
        public ICommand CmdOK => new Command(() =>
        {
            Navigation.PopToRoot();
        });
    }
}
