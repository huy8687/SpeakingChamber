﻿using SpeakingChamber.Model;
using SpeakingChamber.Pages;
using System.Windows.Input;

namespace SpeakingChamber.ViewModel
{
    public class StartExamViewModel : BaseViewModel
    {
        public string DecoCode => "CODE: " + DataMaster.CurrentTest?.Code;
        public string DecoName => "Name: " + DataMaster.UserName;
        public string DecoDate => "Date: " + DataMaster.Date;
        public ICommand CmdStart => new Command(()=>
        {
            Navigation.Navigate(new TestPage());
        });
    }
}
