using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SpeakingChamber.Model;
using SpeakingChamber.Pages;

namespace SpeakingChamber.ViewModel
{
    public class OpeningViewModel : BaseViewModel
    {
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
