using SpeakingChamber.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.IO;
using SpeakingChamber.Pages;

namespace SpeakingChamber.ViewModel
{
    public class InstructionViewModel : BaseViewModel
    {
        public Visibility ShowMenu { get; private set; } = Visibility.Visible;
        public Visibility ShowVideo { get; private set; } = Visibility.Hidden;
        public string VideoURL { get; private set; }

        public ICommand CmdStart => new Command(() =>
        {
            ShowMenu = Visibility.Hidden;
            ShowVideo = Visibility.Visible;
            VideoURL = _videoPath;
        });
        public ICommand CmdSkipInstruction => new Command(() =>
        {
            Navigation.Navigate(new TestSelectionPage());
        });
        public ICommand CmdSkipVideo => new Command(() =>
        {
            Navigation.Navigate(new TestSelectionPage());
        });

        private readonly string _videoPath = Path.Combine(DataMaster.Setting.LocalPath, "video", "instruction.mp4");
        private readonly MediaElement _videoView;

        public InstructionViewModel(MediaElement videoView)
        {
            _videoView = videoView;
        }

        public override async Task Appearing()
        {
            await base.Appearing();
            if (File.Exists(_videoPath))
            {
                _videoView.MediaOpened += _videoView_MediaOpened;
                _videoView.MediaFailed += _videoView_MediaFailed;
                _videoView.MediaEnded += _videoView_MediaEnded;
            }
            else
            {
                Navigation.Navigate(new TestSelectionPage());
            }
        }

        public override async Task Disappearing()
        {
            await base.Disappearing();
            _videoView.Stop();
            _videoView.MediaOpened -= _videoView_MediaOpened;
            _videoView.MediaFailed -= _videoView_MediaFailed;
            _videoView.MediaEnded -= _videoView_MediaEnded;
        }

        private void _videoView_MediaEnded(object sender, RoutedEventArgs e)
        {
            Navigation.Navigate(new TestSelectionPage());
        }

        private void _videoView_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Navigation.Navigate(new TestSelectionPage());
        }

        private void _videoView_MediaOpened(object sender, RoutedEventArgs e)
        {
        }
    }
}
