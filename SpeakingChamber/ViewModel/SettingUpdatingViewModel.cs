using System.Windows.Input;
using SpeakingChamber.Model;
using Microsoft.Win32;
using System.Windows;
using SpeakingChamber.Extension;
using NAudio.Wave;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpeakingChamber.ViewModel
{
    public class SettingUpdatingViewModel : BaseViewModel
    {
        public int NumberTest { get; set; } = DataMaster.Tests.Count;
        public string OnlineURL { get; set; } = DataMaster.Setting.OnlineUrl;
        public string LocalPath { get; set; } = DataMaster.Setting.LocalPath;
        public string NetworkPath { get; set; } = DataMaster.Setting.NetworkPath;

        public ICommand CmdImport => new Command(() =>
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                var result = DataMaster.ImportTest(ofd.FileName);
                NumberTest = DataMaster.Tests.Count;
                if (result != null)
                {
                    MessageBox.Show($"Successfully import {result.Count} exams to database");
                }
            }
        });

        public ICommand CmdSave => new Command(() =>
        {
            if (!string.IsNullOrWhiteSpace(OnlineURL) && !Utils.CheckUrl(OnlineURL))
            {
                MessageBox.Show("OnlineURL is not correct");
                OnlineURL = "";
            }
            if (!Utils.CheckPath(LocalPath))
            {
                MessageBox.Show("LocalPath is not correct");
                return;
            }
            if (string.IsNullOrWhiteSpace(NetworkPath))
            {
                NetworkPath = string.Empty;
            }
            else if (!Utils.CheckPath(NetworkPath))
            {
                MessageBox.Show("NetworkPath is not correct");
                return;
            }
            if (DataMaster.SaveSetting(OnlineURL, LocalPath, NetworkPath))
            {
                MessageBox.Show("Succesfully apply your settings..");
                Navigation.PopToRoot();
            }
            else
            {
                MessageBox.Show("Failed apply your settings..");
            }
        });

        public ICommand CmdCancel => new Command(() =>
        {
            Navigation.PopToRoot();
        });

        public IList<WaveInCapabilities> InputSources { get; private set; }
        public WaveInCapabilities? SelectedInput { get; set; }
        public Visibility VisibleInputSource => (InputSources != null && InputSources.Count > 1) ? Visibility.Visible : Visibility.Hidden;

        public override async Task Appearing()
        {
            await base.Appearing();
            var temp = new List<WaveInCapabilities>();
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                temp.Add(WaveIn.GetCapabilities(i));
            }
            InputSources = temp;
            if (DataMaster.Setting.DevNumber.HasValue && InputSources.Count > DataMaster.Setting.DevNumber.Value)
            {
                SelectedInput = InputSources[DataMaster.Setting.DevNumber.Value];
            }
        }

        public void OnSelectedInputChanged()
        {
            var devNumber = InputSources.IndexOf(SelectedInput.Value);

            DataMaster.Setting.DevNumber = devNumber;
            DataMaster.Setting.MicChanel = SelectedInput.Value.Channels;
        }
    }
}
