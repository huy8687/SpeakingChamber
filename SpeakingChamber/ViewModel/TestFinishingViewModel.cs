using SpeakingChamber.Model;
using System.Windows.Input;

namespace SpeakingChamber.ViewModel
{
    public class TestFinishingViewModel : BaseViewModel
    {
        public string UserName => DataMaster.UserName;
        public string ExamCode => DataMaster.CurrentTest.Code;
        public string ExamDate => DataMaster.Date;

        public ICommand CmdOK => new Command(()=>
        {

        });
    }
}
