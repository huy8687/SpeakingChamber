using SpeakingChamber.Model;
using System.Windows.Input;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Xml.Serialization;
using SpeakingChamber.Extension;
using System.Windows;

namespace SpeakingChamber.ViewModel
{
    public class TestFinishingViewModel : BaseViewModel
    {
        public string UserName => DataMaster.UserName;
        public string ExamCode => DataMaster.CurrentTest?.Code;
        public string ExamDate => DateTime.Now.ToString("dd/MM/yyyy");

        public Visibility ShowSaving { get; private set; }
        public Visibility ShowClose => ShowSaving == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

        public ICommand CmdOK => new Command(() =>
        {
            Navigation.PopToRoot();
        });

        public override async Task Appearing()
        {
            await base.Appearing();
            ShowSaving = Visibility.Visible;
            await Task.Run(() => PushFileToNetwork()).ContinueWith((task) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (task.Exception != null)
                    {
                        MessageBox.Show("Cannot upload files to network path. Your files are saved in local path.");
                    }
                    ShowSaving = Visibility.Hidden;
                });
            });
        }

        private void PushFileToNetwork()
        {
            if (!Utils.CheckPath(DataMaster.Setting.UserLocalPath) || !Utils.CheckPath(DataMaster.Setting.NetworkPath))
            {
                return;
            }
            var di = new DirectoryInfo(DataMaster.Setting.UserLocalPath);
            var diNetwork = new DirectoryInfo(DataMaster.Setting.NetworkPath);
            if (di.Exists && diNetwork.Exists)
            {
                var today = DateTime.Now.ToString("ddMMyyyy");
                var name = DataMaster.UserNamePath + "_" + DataMaster.UserDobPath;
                diNetwork = new DirectoryInfo(Path.Combine(DataMaster.Setting.NetworkPath, today));
                if (!diNetwork.Exists) diNetwork.Create();
                diNetwork = new DirectoryInfo(Path.Combine(DataMaster.Setting.NetworkPath, today, name));
                if (!diNetwork.Exists) diNetwork.Create();

                foreach (var file in diNetwork.GetFiles())
                {
                    file.Delete();
                }

                foreach (var file in di.GetFiles())
                {
                    File.Copy(file.FullName, Path.Combine(diNetwork.FullName, file.Name));
                }

                DataMaster.SaveCurrentTestFile(diNetwork.FullName);
            }
        }
    }
}
