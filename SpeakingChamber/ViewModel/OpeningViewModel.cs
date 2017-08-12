using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SpeakingChamber.Model;
using SpeakingChamber.Pages;
using System.IO;
using SpeakingChamber.Extension;

namespace SpeakingChamber.ViewModel
{
    public class OpeningViewModel : BaseViewModel
    {
        public Visibility IsShowSetting => IsShowLoading == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        public Visibility IsShowLoading { get; set; } = Visibility.Hidden;

        public Visibility IsShowOnlineURL { get; set; } = Visibility.Hidden;
        public ICommand CmdOpenOnlineURL => new Command(() =>
        {
            System.Diagnostics.Process.Start(DataMaster.Setting.OnlineUrl);
        });

        public Visibility IsShowLoginForm { get; set; } = Visibility.Hidden;
        public string LblName { get; set; }
        public string LblDoB { get; set; }
        public string LblError { get; set; }
        public ICommand CmdStart => new Command(() =>
        {
            LblName = LblName.SupperTrim();

            try
            {
                const string CS_FOLDER = "Checksumxx";
                var cs_In = new DirectoryInfo(CS_FOLDER);
                if (!cs_In.Exists)
                    cs_In.Create();
                try
                {
                    var path = Path.Combine(CS_FOLDER, LblName);
                    File.WriteAllText(path, "test");
                    File.Delete(path);
                }
                catch { throw; }
                finally { cs_In.Delete(true); }
            }
            catch
            {
                LblError = "Name is invalid!";
                return;
            }

            var result = !string.IsNullOrWhiteSpace(LblName) && !string.IsNullOrWhiteSpace(LblDoB);
            LblError = result ? "" : "Please input name & date of birth!";
            if (result)
            {
                DataMaster.UserName = LblName;
                DataMaster.Date = DateTime.Parse(LblDoB).ToString("dd/MM/yyyy");
                Navigation.Navigate(new TestSelectionPage());
            }
        });

        public ICommand CmdSettings => new Command(() =>
        {
            Navigation.Navigate(new SettingUpdatingPage());
        });

        private bool _loaded;

        public override async Task Appearing()
        {
            await base.Appearing();
            if (!_loaded)
            {
                LoadData();
                _loaded = true;
            }
        }

        private async void LoadData()
        {
            IsShowLoading = Visibility.Visible;
            await Task.Run(async () =>
            {
                DataMaster.LoadData();
                await Task.Delay(2000);
            });
            IsShowLoading = Visibility.Hidden;
            var setting = DataMaster.Setting;
            if (setting.OnlineUrl != null && Utils.CheckUrl(setting.OnlineUrl))
            {
                System.Diagnostics.Process.Start(setting.OnlineUrl);
                IsShowOnlineURL = Visibility.Visible;
                IsShowLoginForm = Visibility.Hidden;
            }
            else
            {
                IsShowOnlineURL = Visibility.Hidden;
                IsShowLoginForm = Visibility.Visible;
            }
        }
    }
}
