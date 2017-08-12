using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SpeakingChamber.Model;
using Microsoft.Win32;
using System.Windows;

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
            }
            if (!Utils.CheckPath(LocalPath))
            {
                MessageBox.Show("LocalPath is not correct");
                return;
            }
            if (!Utils.CheckPath(NetworkPath))
            {
                MessageBox.Show("NetworkPath is not correct");
                return;
            }
            if (DataMaster.SaveSetting(OnlineURL, LocalPath, NetworkPath))
            {
                MessageBox.Show("Succesfully apply your settings...");
                // TODO: goto next screen
            }
            else
            {
                MessageBox.Show("Failed apply your settings...");
            }
        });

        public ICommand CmdCancel => new Command(() =>
        {
            // TODO: goto next screen
        });
    }
}
