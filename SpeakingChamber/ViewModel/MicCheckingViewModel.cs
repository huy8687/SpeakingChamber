using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SpeakingChamber.Model;
using System.Windows;
using NAudio.Wave;

namespace SpeakingChamber.ViewModel
{
    public class MicCheckingViewModel : BaseViewModel
    {
        public IList<WaveInCapabilities> InputSources { get; private set; }
        public WaveInCapabilities SelectedInput { get; set; }
        public bool EnableComplete { get; set; } = true;

        public Visibility VisibleConfirm => EnableComplete ? Visibility.Hidden : Visibility.Visible;
        public Visibility VisibleInputSource => (InputSources != null && InputSources.Count > 0) ? Visibility.Visible : Visibility.Hidden;

        public ICommand CmdComplete => new Command(() =>
        {
            // TODO: logic complete
            EnableComplete = false;
        });

        public ICommand CmdYes => new Command(() =>
        {

        });

        public ICommand CmdNo => new Command(() =>
        {

        });

        public override async Task Appearing()
        {
            await base.Appearing();
            var temp = new List<WaveInCapabilities>();
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                temp.Add(WaveIn.GetCapabilities(i));
            }
            temp.Add(new WaveInCapabilities() { });
            InputSources = temp;
            if (temp.Any())
            {
                SelectedInput = InputSources[0];
            }
        }

        public override async Task Disappearing()
        {
            await base.Disappearing();
        }
    }
}
