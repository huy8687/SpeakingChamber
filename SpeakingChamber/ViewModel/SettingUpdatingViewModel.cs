using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SpeakingChamber.Model;

namespace SpeakingChamber.ViewModel
{
    public class SettingUpdatingViewModel : BaseViewModel
    {
        public int NumberTest { get; set; }
        public string OnlineURL { get; set; }
        public string LocalPath { get; set; }
        public string NetworkPath { get; set; }

        public ICommand CmdImport => new Command(() =>
        {
            // TODO handle show import
        });
    }
}
