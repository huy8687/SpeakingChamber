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
        public Visibility IsShowLoading { get; set; } = Visibility.Hidden;

        public Visibility IsShowOnlineURL { get; set; } = Visibility.Hidden;
        public ICommand CmdOpenOnlineURL => new Command(() =>
        {
            System.Diagnostics.Process.Start(DataMaster.Setting.OnlineUrl);
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
            else
            {
                ShowUserLogin();
            }
        }

        private async void LoadData()
        {
            IsShowLoading = Visibility.Visible;
            await Task.Run(() =>
            {
                DataMaster.LoadData();
            });
            IsShowLoading = Visibility.Hidden;
            ShowUserLogin();
        }

        private void ShowUserLogin()
        {
            var setting = DataMaster.Setting;
            if (setting.OnlineUrl != null && Utils.CheckUrl(setting.OnlineUrl))
            {
                System.Diagnostics.Process.Start(setting.OnlineUrl);
                IsShowOnlineURL = Visibility.Visible;
            }
            else
            {
                if (Utils.CheckPath(setting.LocalPath))
                {
                    Navigation.Navigate(new UserLoginPage());
                }
                else
                {
                    MessageBox.Show("Please create LocalPath");
                }

            }
        }
    }
}
