using SpeakingChamber.Model;
using SpeakingChamber.Pages;
using System.Collections.Generic;
using System.Windows.Input;

namespace SpeakingChamber.ViewModel
{
    public class TestSelectionViewModel : BaseViewModel
    {
        public IList<Test> Tests => DataMaster.Tests;
        public ICommand CmdTestCode { get; private set; }
        public TestSelectionViewModel()
        {
            CmdTestCode = new Command<Test>((test =>
            {
                DataMaster.CurrentTest = test;
                Navigation.Navigate(new MicCheckingPage());
            }));
        }
    }
}
