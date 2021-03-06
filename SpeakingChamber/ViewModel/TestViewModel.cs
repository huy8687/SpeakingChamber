﻿using SpeakingChamber.Model;
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
using NAudio.Lame;
using SpeakingChamber.Extension;

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

        public Visibility ShowRecording { get; set; } = Visibility.Hidden;
        public Visibility ShowQuestionVideo { get; set; } = Visibility.Hidden;
        public string VideoURL { get; private set; }

        public Visibility ShowQuestionContent { get; set; } = Visibility.Hidden;

        public Visibility ShowPreparation { get; set; } = Visibility.Hidden;

        public string BtnContinueText { get; set; }
        public string TbNoteForBtnCountinue { get; set; }

        private bool _isFinish => _parts != null && _parts.Count == 0 && _questions != null && _questions.Count == 0;

        public ICommand CmdContinue => new Command(() =>
        {
            this.Log("CmdContinue Start");
            StopRecord();
            NextQuestion();
            this.Log("CmdContinue End");
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
            DataMaster.SaveCurrentTestFile(DataMaster.Setting.UserLocalPath);
            NextQuestion();
        }

        public override async Task Disappearing()
        {
            this.Log("Disappearing Start");
            await base.Disappearing();
            StopRecord();
            _timer.Stop();
            _timer.Tick -= _timer_Tick;
            VideoURL = null;
            _videoView.MediaOpened -= _videoView_MediaOpened;
            _videoView.MediaFailed -= _videoView_MediaFailed;
            _videoView.MediaEnded -= _videoView_MediaEnded;
            _parts = null;
            _questions = null;
            this.Log("Disappearing End");
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
                throw new InvalidOperationException("CurQuestion cannot be null");
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (--RemainTime <= 0)
            {
                _timer.Stop();

                switch (_lastState)
                {
                    case EDisplayType.ShowQuestionVideo:
                        break;
                    case EDisplayType.ShowOutOfTime:
                        // TIMEOUT DONE
                        NextQuestion();
                        break;
                    case EDisplayType.ShowQuestionContent:
                        // TIMEOUT => show timeout in 3s
                        StopRecord();
                        SetupShowTimeOut();
                        break;
                    case EDisplayType.ShowPreparation:
                        if (!string.IsNullOrWhiteSpace(CurQuestion.Video2) && File.Exists(CurQuestion.Video2))
                            SetupShowEndOfPreparation();
                        else
                            SetupShowQuestion();
                        break;
                }
            }
        }

        private void NextQuestion()
        {
            this.Log("NextQuestion Start");
            if (_questions != null && _questions.Count > 0)
                CurQuestion = _questions.Dequeue();
            else
            {
                if (_parts.Count > 0)
                    CurPart = _parts.Dequeue();
                else
                {
                    ShowQuestion = Visibility.Hidden;
                    //if (_convertMp3Task != null && !_convertMp3Task.IsCompleted)
                    //    await _convertMp3Task.ContinueWith(task =>
                    //        {
                    //            Application.Current.Dispatcher.Invoke(() => Navigation.Navigate(new TestFinishingPage()));
                    //        });
                    //else
                    Navigation.Navigate(new TestFinishingPage());
                }
            }
            this.Log("NextQuestion End");
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
        private EDisplayType _lastState;

        private void SetupShowVideo()
        {
            HandleDisplay(EDisplayType.ShowQuestionVideo);
            if (VideoURL != CurQuestion.Video)
                VideoURL = CurQuestion.Video;
            else
                _videoView.Position = new TimeSpan(0);
        }

        private void SetupShowPreparation()
        {
            HandleDisplay(EDisplayType.ShowPreparation);
            RemainTime = CurQuestion.PreparationTime;
            _timer.Start();
        }


        private void SetupShowEndOfPreparation()
        {
            HandleDisplay(EDisplayType.ShowEndOfPreparation);
            if (VideoURL != CurQuestion.Video2)
                VideoURL = CurQuestion.Video2;
            else
                _videoView.Position = new TimeSpan(0);
        }

        private void SetupShowQuestion()
        {
            if (CurQuestion.PreparationTime > 0 && _lastState != EDisplayType.ShowPreparation && _lastState != EDisplayType.ShowEndOfPreparation)
            {
                SetupShowPreparation();
            }
            else
            {
                BtnContinueText = !_isFinish ? "Stop recording" : "Complete the exam";
                TbNoteForBtnCountinue = !_isFinish ? "and move on to the next question" : string.Empty;
                HandleDisplay(EDisplayType.ShowQuestionContent);
                RemainTime = CurQuestion.Duration;

                _timer.Start();
                StartRecord();
            }
        }

        private void SetupShowTimeOut()
        {
            HandleDisplay(EDisplayType.ShowOutOfTime);
            RemainTime = 3;
            _timer.Start();
        }

        private void HandleDisplay(EDisplayType type)
        {
            _lastState = type;
            switch (type)
            {
                case EDisplayType.ShowQuestionVideo:
                    ShowPart = ShowQuestion = ShowQuestionVideo = Visibility.Visible;
                    ShowOutOfTime = ShowQuestionContent = ShowPreparation = ShowRecording = Visibility.Hidden;
                    break;
                case EDisplayType.ShowOutOfTime:
                    ShowQuestion = ShowQuestionVideo = ShowQuestionContent = ShowPart = ShowPreparation = ShowRecording = Visibility.Hidden;
                    ShowOutOfTime = Visibility.Visible;
                    break;
                case EDisplayType.ShowQuestionContent:
                    ShowPart = ShowQuestion = ShowQuestionContent = ShowRecording = Visibility.Visible;
                    ShowOutOfTime = ShowQuestionVideo = ShowPreparation = Visibility.Hidden;
                    break;
                case EDisplayType.ShowPreparation:
                    ShowPart = ShowQuestion = ShowQuestionContent = ShowPreparation = Visibility.Visible;
                    ShowOutOfTime = ShowQuestionVideo = ShowRecording = Visibility.Hidden;
                    break;
                case EDisplayType.ShowEndOfPreparation:
                    ShowPart = ShowQuestion = ShowQuestionVideo = Visibility.Visible;
                    ShowOutOfTime = ShowQuestionContent = ShowPreparation = ShowRecording = Visibility.Hidden;
                    break;
            }
        }
    }

    // Audio Record
    public partial class TestViewModel
    {
        private WaveIn _inputStream;
        private WaveFileWriter _waveWriter;

        private readonly IList<string> _files = new List<string>();
        private readonly Queue<string> _queueFiles = new Queue<string>();
        private string _curPath;
        private bool _isRunning;
        private Task _convertMp3Task;

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

                var recordedFileName = $"{DateTime.Now.ToString("yyyyMMdd")}_{DataMaster.UserNamePath}_{DataMaster.UserDobPath}_{_curTest.Code}_{_curTest.Parts.IndexOf(CurPart) + 1}_{CurPart.Questions.IndexOf(CurQuestion) + 1}.wav";
                _curPath = Path.Combine(DataMaster.Setting.UserLocalPath, recordedFileName);
                CurQuestion.LocalRecordedPath = _curPath.Substring(0, _curPath.Length - 4) + ".mp3";

                _waveWriter = new WaveFileWriter(_curPath, _inputStream.WaveFormat);
                _inputStream.StartRecording();
            }
            catch
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
            ConvertToMp3();
        }

        private void ConvertToMp3()
        {
            if (string.IsNullOrWhiteSpace(_curPath) || _files.Contains(_curPath))
            {
                return;
            }
            _files.Add(_curPath);
            _queueFiles.Enqueue(_curPath);
            //if (!_isRunning)
            //{
            //    _convertMp3Task = Task.Run(() =>
            //    {
            //        _isRunning = true;
            //        while (_queueFiles.Count > 0)
            //        {
            var wavPath = _queueFiles.Dequeue();
            var mp3Path = wavPath.Substring(0, wavPath.Length - 4) + ".mp3";
            using (var reader = new AudioFileReader(wavPath))
            using (var writer = new LameMP3FileWriter(mp3Path, reader.WaveFormat, 128))
                reader.CopyTo(writer);
            File.Delete(wavPath);
            //        }
            //        _isRunning = false;
            //    }).ContinueWith((task) => _isRunning = false); ;
            //}
        }
    }

    enum EDisplayType
    {
        ShowQuestionVideo,
        ShowOutOfTime,
        ShowQuestionContent,
        ShowPreparation,
        ShowEndOfPreparation
    }
}
