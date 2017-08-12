using SpeakingChamber.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using NAudio.Wave;
using System.Windows.Input;
using SpeakingChamber.Pages;
using System.Globalization;

namespace SpeakingChamber.ViewModel
{
    public partial class TestViewModel : BaseViewModel
    {
        public Visibility ShowOutOfTime { get; set; } = Visibility.Hidden;

        public Visibility ShowPart { get; set; } = Visibility.Hidden;
        public Part CurPart { get; private set; }

        public Visibility ShowQuestion { get; set; } = Visibility.Hidden;
        public Question CurQuestion { get; private set; }
        public string DecoRemainTime => TimeSpan.FromSeconds(RemainTime).ToString(@"mm\:ss");
        public int RemainTime { get; private set; }

        public Visibility ShowQuestionVideo { get; set; } = Visibility.Hidden;
        public string VideoURL { get; private set; }

        public Visibility ShowQuestionContent { get; set; } = Visibility.Hidden;

        public ICommand CmdContinue => new Command(() =>
        {
            StopRecord();
            NextQuestion();
        });

        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly MediaElement _videoView;

        private readonly Test _curTest;
        private Queue<Part> _parts;
        private Queue<Question> _questions;

        public TestViewModel(MediaElement videoView)
        {
            _videoView = videoView;
            _curTest = DataMaster.CurrentTest;
        }

        public override async Task Appearing()
        {
            await base.Appearing();

            var di = new DirectoryInfo(DataMaster.Setting.UserLocalPath);
            if (di.Exists)
            {
                foreach (var file in di.GetFiles()) file.Delete();
                foreach (var dir in di.GetDirectories()) dir.Delete(true);
            }
            else
            {
                di.Create();
            }


            _videoView.MediaOpened += _videoView_MediaOpened;
            _videoView.MediaFailed += _videoView_MediaFailed;
            _videoView.MediaEnded += _videoView_MediaEnded;

            _timer.Tick += _timer_Tick;
            _timer.Interval = TimeSpan.FromSeconds(1);

            _parts = new Queue<Part>(_curTest.Parts);
            NextQuestion();
        }

        public override async Task Disappearing()
        {
            await base.Disappearing();
            StopRecord();
            _timer.Stop();
            _timer.Tick -= _timer_Tick;
            _videoView.Stop();
            _videoView.MediaOpened -= _videoView_MediaOpened;
            _videoView.MediaFailed -= _videoView_MediaFailed;
            _videoView.MediaEnded -= _videoView_MediaEnded;
            _parts = null;
            _questions = null;
        }

        public void OnCurPartChanged()
        {
            if (CurPart != null)
            {
                _questions = new Queue<Question>(CurPart.Questions);
                NextQuestion();
            }
        }

        public void OnCurQuestionChanged()
        {
            if (CurQuestion != null)
                if (!string.IsNullOrWhiteSpace(CurQuestion.Video) && File.Exists(CurQuestion.Video))
                    SetupShowVideo();
                else
                    SetupShowQuestion();
            else
                Navigation.Navigate(new TestFinishingPage());
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (--RemainTime <= 0)
            {
                _timer.Stop();

                if (ShowQuestion == Visibility.Visible)
                {
                    // TIMEOUT => show timeout in 3s
                    StopRecord();
                    SetupShowTimeOut();
                }
                else if (ShowOutOfTime == Visibility.Visible)
                {
                    // TIMEOUT DONE
                    NextQuestion();
                }
            }
        }

        private void NextQuestion()
        {
            if (_questions != null && _questions.Count > 0)
                CurQuestion = _questions.Dequeue();
            else
                if (_parts.Count > 0)
                CurPart = _parts.Dequeue();
            else
                Navigation.Navigate(new TestFinishingPage());
        }

        private void _videoView_MediaOpened(object sender, RoutedEventArgs e)
        {
            // Something maybe
        }

        private void _videoView_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            //MessageBox.Show($"Fail to play Video file: {VideoURL}");
            Application.Current.Dispatcher.Invoke(SetupShowQuestion);
        }

        private void _videoView_MediaEnded(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(SetupShowQuestion);
        }
    }

    // Display content
    public partial class TestViewModel
    {
        private void SetupShowVideo()
        {
            HandleDisplay(EDisplayType.ShowQuestionVideo);
            VideoURL = CurQuestion.Video;
        }

        private void SetupShowQuestion()
        {
            HandleDisplay(EDisplayType.ShowQuestionContent);
            RemainTime = CurQuestion.Duration;

            _timer.Start();
            StartRecord();
        }

        private void SetupShowTimeOut()
        {
            HandleDisplay(EDisplayType.ShowOutOfTime);
            RemainTime = 3;
            _timer.Start();
        }

        private void HandleDisplay(EDisplayType type)
        {
            switch (type)
            {
                case EDisplayType.ShowQuestionVideo:
                    ShowPart = ShowQuestion = ShowQuestionVideo = Visibility.Visible;
                    ShowOutOfTime = ShowQuestionContent = Visibility.Hidden;
                    break;
                case EDisplayType.ShowOutOfTime:
                    ShowPart = ShowQuestion = ShowQuestionVideo = ShowQuestionContent = Visibility.Hidden;
                    ShowOutOfTime = Visibility.Visible;
                    break;
                case EDisplayType.ShowQuestionContent:
                    ShowPart = ShowQuestion = ShowQuestionContent = Visibility.Visible;
                    ShowOutOfTime = ShowQuestionVideo = Visibility.Hidden;
                    break;
            }
        }
    }

    // Audio Record
    public partial class TestViewModel
    {
        private WaveIn _inputStream;
        private WaveFileWriter _waveWriter;

        public void StartRecord()
        {
            try
            {
                _inputStream = new WaveIn
                {
                    DeviceNumber = DataMaster.Setting.DevNumber.Value,
                    WaveFormat = new WaveFormat(44100, DataMaster.Setting.MicChanel.Value)
                };
                _inputStream.DataAvailable += InputStreamOnDataAvailable;

                var dob = DateTime.ParseExact(DataMaster.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("ddMMyyyy");
                var path = Path.Combine(DataMaster.Setting.UserLocalPath, DateTime.Now.ToString("ddMMyyyy") + "_" + DataMaster.UserName.Trim() + "_" + dob + "_" + _curTest.Code + "_" + _curTest.Parts.IndexOf(CurPart) + "_" + CurPart.Questions.IndexOf(CurQuestion) + ".wav");
                _waveWriter = new WaveFileWriter(path, _inputStream.WaveFormat);
                _inputStream.StartRecording();
            }
            catch (Exception ex)
            {
                StopRecord();
                NextQuestion();
            }
        }

        private void InputStreamOnDataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                if (_waveWriter != null)
                {
                    _waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
                    _waveWriter.Flush();
                }
            }
            catch (Exception ex)
            {
                StopRecord();
                NextQuestion();
            }
        }

        private void StopRecord()
        {
            _timer.Stop();
            if (_inputStream != null)
            {
                _inputStream.StopRecording();
                _inputStream.DataAvailable -= InputStreamOnDataAvailable;
                _inputStream.Dispose();
                _inputStream = null;
            }
            if (_waveWriter != null)
            {
                _waveWriter.Close();
                _waveWriter.Dispose();
                _waveWriter = null;
            }
        }
    }

    enum EDisplayType
    {
        ShowQuestionVideo,
        ShowOutOfTime,
        ShowQuestionContent
    }
}
