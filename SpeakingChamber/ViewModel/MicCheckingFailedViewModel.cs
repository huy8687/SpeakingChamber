using SpeakingChamber.Extension;
using SpeakingChamber.Model;
using SpeakingChamber.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
